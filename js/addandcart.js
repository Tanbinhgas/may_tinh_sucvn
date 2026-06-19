// ============================================================
//  addandcart.js - Giỏ hàng dùng MySQL
//  Hỗ trợ: kiểm tra tồn kho, thành tiền, tổng tiền realtime
// ============================================================

const CART_API = "/may_tinh_sucvn/php/api_cart.php";

// ── THÊM VÀO GIỎ HÀNG ──────────────────────────────────────
document.querySelectorAll(".my-cart-btn").forEach((btn) => {
  btn.addEventListener("click", addToCart);
});

async function addToCart(e) {
  e.preventDefault();
  const btn = e.target;

  const fd = new FormData();
  fd.append("action", "add");
  fd.append("product_id", btn.dataset.id);
  fd.append("name", btn.dataset.name);
  const cleanPrice =
    parseInt(btn.dataset.price.toString().replace(/[.,]/g, "")) || 0;
  fd.append("price", cleanPrice);
  fd.append("quantity", btn.dataset.quantity || 1);
  fd.append("image", btn.dataset.image || "");
  fd.append("summary", btn.dataset.summary || "");

  const res = await fetch(CART_API, { method: "POST", body: fd });
  const data = await res.json();

  if (!data.success && data.require_login) {
    if (confirm("Bạn cần đăng nhập để thêm vào giỏ hàng. Đăng nhập ngay?")) {
      window.location.href = "/may_tinh_sucvn/php/login.php";
    }
    return;
  }

  if (data.success) {
    showToast("🛒 Đã thêm vào giỏ hàng!");
    updateCartBadge(data.cart_count);
  } else {
    alert(data.message);
  }
}

// ── MỞ GIỎ HÀNG ────────────────────────────────────────────
const cartIcon = document.querySelector(".my-cart-icon");
if (cartIcon) cartIcon.addEventListener("click", openCart);

async function openCart() {
  const res = await fetch(CART_API + "?action=get");
  const data = await res.json();

  if (!data.success && data.require_login) {
    if (confirm("Bạn cần đăng nhập để xem giỏ hàng. Đăng nhập ngay?")) {
      window.location.href = "/may_tinh_sucvn/php/login.php";
    }
    return;
  }

  const items = data.items || [];

  document.querySelector(".cart-overlay")?.remove();

  const overlay = document.createElement("div");
  overlay.className = "cart-overlay";
  overlay.innerHTML = `
        <div class="cart-popup">
            <div class="cart-header">
                <h3>🛒 Giỏ hàng</h3>
                <div class="cart-header-actions">
                    <a href="/may_tinh_sucvn/php/my_orders.php" class="orders-link">📋 Lịch sử đơn hàng</a>
                    <button class="close-cart-btn">✕</button>
                </div>
            </div>
            <div class="cart-body">
                ${
                  items.length === 0
                    ? '<p class="cart-empty">Giỏ hàng trống.</p>'
                    : `<ul class="cart-list">
${items
  .map(
    (item) => `
    <li class="cart-item" data-id="${item.product_id}">
        <img src="${item.image || ""}" alt="${item.name}" onerror="this.style.display='none'">
        <div class="cart-item-info">
            <p class="cart-item-name">${item.name}</p>
            <p class="cart-item-price">${Number(item.price).toLocaleString("vi-VN")}₫</p>
            <p class="item-subtotal">
                Thành tiền:
                <span id="subtotal-${item.product_id}">
                    ${(item.price * item.quantity).toLocaleString("vi-VN")}₫
                </span>
            </p>
            <div class="cart-item-controls">
                <button class="qty-btn qty-minus" data-id="${item.product_id}">−</button>
                <input class="qty-input" type="number" min="1" value="${item.quantity}" data-id="${item.product_id}">
                <button class="qty-btn qty-plus" data-id="${item.product_id}">+</button>
                <button class="remove-btn" data-id="${item.product_id}">🗑</button>
            </div>
        </div>
    </li>
`,
  )
  .join("")}
                       </ul>
                       <div class="cart-footer">
                           <p class="cart-total">Tổng: <b id="cartTotal">${calcTotal(items)}</b></p>
                           <button class="checkout-btn">Đặt hàng</button>
                       </div>`
                }
            </div>
        </div>
    `;
  document.body.appendChild(overlay);

  overlay
    .querySelector(".close-cart-btn")
    .addEventListener("click", () => overlay.remove());
  overlay.addEventListener("click", (e) => {
    if (e.target === overlay) overlay.remove();
  });

  if (items.length === 0) return;

  overlay.querySelectorAll(".qty-minus").forEach((btn) =>
    btn.addEventListener("click", async () => {
      const id = btn.dataset.id;
      const inp = overlay.querySelector(`.qty-input[data-id="${id}"]`);
      const newQty = Math.max(0, parseInt(inp.value) - 1);
      inp.value = newQty;
      await updateQty(id, newQty, overlay);
      if (newQty === 0)
        overlay.querySelector(`.cart-item[data-id="${id}"]`)?.remove();
    }),
  );

  overlay.querySelectorAll(".qty-plus").forEach((btn) =>
    btn.addEventListener("click", async () => {
      const id = btn.dataset.id;
      const inp = overlay.querySelector(`.qty-input[data-id="${id}"]`);
      const newQty = parseInt(inp.value) + 1;
      inp.value = newQty;
      await updateQty(id, newQty, overlay);
    }),
  );

  overlay.querySelectorAll(".qty-input").forEach((inp) =>
    inp.addEventListener("change", async () => {
      const qty = Math.max(0, parseInt(inp.value));
      inp.value = qty;
      await updateQty(inp.dataset.id, qty, overlay);
      if (qty === 0)
        overlay
          .querySelector(`.cart-item[data-id="${inp.dataset.id}"]`)
          ?.remove();
    }),
  );

  overlay.querySelectorAll(".remove-btn").forEach((btn) =>
    btn.addEventListener("click", async () => {
      const fd = new FormData();
      fd.append("action", "remove");
      fd.append("product_id", btn.dataset.id);
      const res = await fetch(CART_API, { method: "POST", body: fd });
      const data = await res.json();
      overlay
        .querySelector(`.cart-item[data-id="${btn.dataset.id}"]`)
        ?.remove();
      document.getElementById("cartTotal").textContent =
        Number(data.total).toLocaleString("vi-VN") + "₫";
      if (!overlay.querySelector(".cart-item")) {
        overlay.querySelector(".cart-body").innerHTML =
          '<p class="cart-empty">Giỏ hàng trống.</p>';
      }
    }),
  );

  overlay.querySelector(".checkout-btn")?.addEventListener("click", () => {
    overlay.remove();
    window.location.href = "/may_tinh_sucvn/php/checkout.php";
  });
}

// ── CẬP NHẬT SỐ LƯỢNG ──────────────────────────────────────
async function updateQty(product_id, quantity, overlay) {
  const fd = new FormData();
  fd.append("action", "update");
  fd.append("product_id", product_id);
  fd.append("quantity", quantity);

  const res = await fetch(CART_API, { method: "POST", body: fd });
  const data = await res.json();

  if (!data.success) {
    alert(data.message);
    if (data.stock !== undefined) {
      const inp = overlay.querySelector(`.qty-input[data-id="${product_id}"]`);
      if (inp) inp.value = data.stock;
    }
    return;
  }

  const row = overlay.querySelector(`.cart-item[data-id="${product_id}"]`);
  if (row) {
    const price = Number(
      row.querySelector(".cart-item-price").textContent.replace(/[^\d]/g, ""),
    );
    const subtotal = price * quantity;
    const subtotalEl = row.querySelector(`#subtotal-${product_id}`);
    if (subtotalEl) {
      subtotalEl.textContent = subtotal.toLocaleString("vi-VN") + "₫";
    }
  }

  const totalEl = document.getElementById("cartTotal");
  if (totalEl) {
    totalEl.textContent = Number(data.total).toLocaleString("vi-VN") + "₫";
  }

  if (data.cart_count !== undefined) {
    updateCartBadge(data.cart_count);
  }
}

function calcTotal(items) {
  const total = items.reduce((sum, i) => sum + i.price * i.quantity, 0);
  return Number(total).toLocaleString("vi-VN") + "₫";
}

function showToast(msg) {
  const t = document.createElement("div");
  t.className = "cart-toast";
  t.textContent = msg;
  document.body.appendChild(t);
  setTimeout(() => t.classList.add("show"), 10);
  setTimeout(() => {
    t.classList.remove("show");
    setTimeout(() => t.remove(), 300);
  }, 2500);
}

function updateCartBadge(count) {
  let badge = document.querySelector(".cart-badge");
  if (!badge) {
    const icon = document.querySelector(".my-cart-icon");
    if (icon) {
      badge = document.createElement("span");
      badge.className = "cart-badge";
      icon.style.position = "relative";
      icon.appendChild(badge);
    }
  }
  if (badge) badge.textContent = count;
}

(async () => {
  try {
    const res = await fetch(CART_API + "?action=get");
    const data = await res.json();
    if (data.success && data.items.length > 0)
      updateCartBadge(data.items.length);
  } catch {}
})();

// CSS giỏ hàng
const cartStyle = document.createElement("style");
cartStyle.textContent = `
.cart-overlay {
    position: fixed; inset: 0; background: rgba(0,0,0,0.5);
    z-index: 9999; display: flex; align-items: center; justify-content: center;
}
.cart-popup {
    background: #fff; border-radius: 12px; width: 90%; max-width: 480px;
    max-height: 85vh; display: flex; flex-direction: column;
    box-shadow: 0 20px 60px rgba(0,0,0,0.3);
}
.cart-header {
    display: flex; justify-content: space-between; align-items: center;
    padding: 14px 20px; border-bottom: 1px solid #eee; gap: 12px;
}
.cart-header-actions { display: flex; align-items: center; gap: 10px; }
.cart-header h3 { font-size: 1.1rem; }
.close-cart-btn {
    background: none; border: none; font-size: 1.2rem;
    cursor: pointer; color: #888; padding: 4px 8px; border-radius: 4px;
}
.close-cart-btn:hover { background: #f5f5f5; }
.cart-body { overflow-y: auto; flex: 1; padding: 16px 20px; }
.cart-empty { text-align: center; color: #aaa; padding: 30px 0; }
.cart-list { list-style: none; padding: 0; margin: 0; display: flex; flex-direction: column; gap: 14px; }
.cart-item { display: flex; gap: 12px; padding-bottom: 14px; border-bottom: 1px solid #f0f0f0; }
.cart-item img { width: 60px; height: 60px; object-fit: cover; border-radius: 6px; flex-shrink: 0; }
.cart-item-info { flex: 1; }
.cart-item-name { font-weight: 500; font-size: 0.9rem; margin-bottom: 4px; }
.cart-item-price { color: #FAB005; font-weight: 700; font-size: 0.9rem; margin-bottom: 8px; }
.cart-item-controls { display: flex; align-items: center; gap: 6px; }
.qty-btn {
    width: 28px; height: 28px; border: 1px solid #ddd; background: #f8f8f8;
    border-radius: 4px; cursor: pointer; font-size: 1rem; line-height: 1;
}
.qty-btn:hover { background: #FAB005; color: #fff; border-color: #FAB005; }
.qty-input {
    width: 48px; text-align: center; border: 1px solid #ddd;
    border-radius: 4px; padding: 4px; font-size: 0.9rem;
}
.remove-btn {
    margin-left: 6px; background: none; border: none;
    cursor: pointer; font-size: 1.1rem; color: #e74c3c;
}
.cart-footer { padding-top: 14px; border-top: 1px solid #eee; }
.cart-total { font-size: 1rem; margin-bottom: 12px; }
.checkout-btn {
    width: 100%; padding: 12px; background: #FAB005; color: #fff;
    border: none; border-radius: 8px; font-size: 1rem; font-weight: 600;
    cursor: pointer; transition: background 0.2s;
}
.checkout-btn:hover { background: #e6a004; }
.cart-toast {
    position: fixed; bottom: 30px; left: 50%; transform: translateX(-50%) translateY(20px);
    background: #333; color: #fff; padding: 10px 20px; border-radius: 20px;
    font-size: 0.9rem; opacity: 0; transition: all 0.3s; z-index: 99999;
}
.cart-toast.show { opacity: 1; transform: translateX(-50%) translateY(0); }
.cart-actions { display: flex; align-items: center; justify-content: space-between; gap: 10px; margin-top: 12px; }
.orders-link {
    display: inline-flex; align-items: center; gap: 6px;
    color: #555; font-size: 0.85rem; text-decoration: none;
    padding: 8px 12px; border: 1px solid #ddd; border-radius: 6px;
    transition: all 0.2s; white-space: nowrap;
}
.orders-link:hover { border-color: #FAB005; color: #FAB005; background: #fff9e6; }
.cart-empty .orders-link { margin-top: 12px; display: inline-flex; }
.cart-badge {
    position: absolute; top: -8px; right: -8px;
    background: #e74c3c; color: #fff; border-radius: 50%;
    width: 18px; height: 18px; font-size: 0.7rem;
    display: flex; align-items: center; justify-content: center; font-weight: 700;
}
`;
document.head.appendChild(cartStyle);

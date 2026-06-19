$(document).ready(function () {
  let searchTimeout;
  const $searchInput = $("#searchInput");
  const $searchResults = $("#searchResults");
  const $searchContainer = $("#searchContainer");

  $searchInput.on("input", function () {
    clearTimeout(searchTimeout);
    const keyword = $(this).val().trim();

    if (keyword.length < 2) {
      $searchResults.html("").hide();
      return;
    }

    searchTimeout = setTimeout(function () {
      $.ajax({
        url: "php/api_search.php",
        type: "GET",
        data: { keyword: keyword },
        dataType: "json",
        beforeSend: function () {
          $searchResults
            .html('<div class="search-loading">⏳ Đang tìm kiếm...</div>')
            .show();
        },
        success: function (data) {
          if (data.error) {
            $searchResults
              .html('<div class="search-error">⚠️ ' + data.error + "</div>")
              .show();
            return;
          }

          if (data.length === 0) {
            $searchResults
              .html(
                '<div class="search-empty">❌ Không tìm thấy sản phẩm nào cho <strong>"' +
                  keyword +
                  '"</strong></div>',
              )
              .show();
            return;
          }

          let html =
            '<div class="search-result-header">📦 <strong>' +
            data.length +
            '</strong> kết quả tìm thấy cho "<strong>' +
            keyword +
            '"</strong></div>';

          $.each(data, function (index, item) {
            html += `
                            <div class="search-result-item" onclick="scrollToProduct('${item.name}')">
                                <div class="search-result-info">
                                    <h4>${item.name}</h4>
                                    <p class="search-result-brand">🏷️ ${item.brand}</p>
                                    <p class="search-result-price">💰 ${item.price}</p>
                                </div>
                            </div>
                        `;
          });

          $searchResults.html(html).show();
        },
        error: function (xhr, status, error) {
          $searchResults
            .html('<div class="search-error">⚠️ Lỗi: ' + error + "</div>")
            .show();
        },
      });
    }, 300);
  });

  $(document).on("click", function (e) {
    if (
      !$searchContainer.is(e.target) &&
      $searchContainer.has(e.target).length === 0
    ) {
      $searchResults.hide();
    }
  });
});

// Hàm cuộn đến sản phẩm theo tên
function scrollToProduct(productName) {
  // Ẩn kết quả tìm kiếm
  $("#searchResults").hide();

  // Lấy tất cả sản phẩm trên trang
  var products = document.querySelectorAll(".women h6 a, .women h6");

  var found = false;
  products.forEach(function (element) {
    var text = element.textContent.trim();
    // So sánh tên sản phẩm (có thể khớp 1 phần)
    if (text.toLowerCase().includes(productName.toLowerCase())) {
      // Tìm container của sản phẩm
      var productContainer = element.closest(".col-md-3");
      if (productContainer) {
        // Cuộn đến sản phẩm
        productContainer.scrollIntoView({
          behavior: "smooth",
          block: "center",
        });
        // Highlight sản phẩm
        productContainer.style.border = "3px solid #FAB005";
        productContainer.style.borderRadius = "10px";
        productContainer.style.transition = "all 0.3s";
        setTimeout(function () {
          productContainer.style.border = "none";
          productContainer.style.borderRadius = "0";
        }, 3000);
        found = true;
      }
    }
  });

  if (!found) {
    alert(
      '⚠️ Sản phẩm "' +
        productName +
        '" không có trên trang chủ!\nVui lòng tìm trong danh mục sản phẩm.',
    );
  }
}

function toggleChat() {
  let chat = document.getElementById("chatBody");
  if (chat) {
    chat.style.display = chat.style.display === "flex" ? "none" : "flex";
  }
}

document.addEventListener("DOMContentLoaded", function () {
  const chatInput = document.getElementById("chatInput");
  if (chatInput) {
    chatInput.addEventListener("keypress", function (e) {
      if (e.key === "Enter") sendMessage();
    });
  }
});

async function sendMessage() {
  const input = document.getElementById("chatInput");
  const chat = document.getElementById("chatContent");
  const userText = input.value.trim();

  if (!userText) return;

  chat.innerHTML += `
        <div style="text-align: right; margin-bottom: 12px;">
            <span style="background: #ce1212; color: white; padding: 8px 14px; border-radius: 16px 16px 0 16px; display: inline-block; max-width: 75%; font-size: 14px;">
                ${userText}
            </span>
        </div>
    `;
  input.value = "";
  chat.scrollTop = chat.scrollHeight;

  const loadingMsg = document.createElement("div");
  loadingMsg.style.marginBottom = "12px";
  loadingMsg.style.textAlign = "left";
  loadingMsg.innerHTML = `
        <span style="background: #f1f5f9; color: #334155; padding: 8px 14px; border-radius: 16px 16px 16px 0; display: inline-block; max-width: 75%; font-size: 14px;">
            <b>Bot:</b> Đang suy nghĩ...
        </span>
    `;
  chat.appendChild(loadingMsg);
  chat.scrollTop = chat.scrollHeight;

  try {
    // ĐÃ SỬA: thêm "php/" vào đường dẫn
    const response = await fetch("/api/chatbot", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ question: userText }),
    });

    const data = await response.json();
    if (data.success) {
      loadingMsg.innerHTML = `
                <span style="background: #fee2e2; color: #ce1212; padding: 8px 14px; border-radius: 16px 16px 16px 0; display: inline-block; max-width: 75%; font-size: 14px; white-space: pre-line;">
                    <b>Bot:</b> ${data.reply}
                </span>
            `;
    } else {
      loadingMsg.innerHTML = `
                <span style="background: #fee2e2; color: #991b1b; padding: 8px 14px; border-radius: 16px 16px 16px 0; display: inline-block; max-width: 75%; font-size: 14px;">
                    <b>Bot:</b> ${data.message}
                </span>
            `;
    }
  } catch (error) {
    loadingMsg.innerHTML = `
            <span style="background: #fee2e2; color: #991b1b; padding: 8px 14px; border-radius: 16px 16px 16px 0; display: inline-block; max-width: 75%; font-size: 14px;">
                <b>Bot:</b> Lỗi kết nối server.
            </span>
        `;
  }
  chat.scrollTop = chat.scrollHeight;
}

<?php
header('Content-Type: application/json; charset=utf-8');

// 1. Kết nối database - cùng thư mục nên dùng __DIR__ trực tiếp
require_once __DIR__ . '/db.php'; 

// ========== CẤU HÌNH API ==========
define('GEMINI_API_KEY', 'AIzaSyC7EQr_OzfCyCZtLgoyaIIXgHd65Ok2nk0');
define('GEMINI_MODEL', 'gemini-2.5-flash');

// 2. Lấy dữ liệu sản phẩm từ DB
$store_data = "Danh sách linh kiện máy tính tại TKL Computer:\n";
try {
    if (isset($pdo)) {
        $stmt = $pdo->query("SELECT name, price, brand FROM products WHERE is_active = 1");
        $products = $stmt->fetchAll(PDO::FETCH_ASSOC);
        
        if (count($products) > 0) {
            foreach ($products as $prod) {
                $store_data .= "- " . $prod['name'] . " | Hãng: " . $prod['brand'] . " | Giá: " . number_format($prod['price']) . "đ\n";
            }
        } else {
            $store_data = "Hiện tại cửa hàng chưa có sản phẩm nào.";
        }
    } else {
        $store_data = "Chưa kết nối database, bot tư vấn kiến thức chung.";
    }
} catch (Exception $e) {
    $store_data = "Hiện tại chưa có danh sách linh kiện cập nhật.";
}

// 3. Đọc câu hỏi từ khung chat
$inputData = json_decode(file_get_contents('php://input'), true);
$userQuestion = isset($inputData['question']) ? trim($inputData['question']) : '';

if (empty($userQuestion)) {
    echo json_encode(['success' => false, 'message' => 'Bạn chưa nhập câu hỏi']);
    exit;
}

// 4. Thiết lập prompt cho AI
$systemInstruction = "Bạn là trợ lý ảo chuyên nghiệp của TKL Computer. Dưới đây là kho linh kiện của cửa hàng:\n"
                    . $store_data . "\n"
                    . "QUY TẮC:\n"
                    . "1. Chỉ tư vấn dựa trên danh sách sản phẩm có sẵn.\n"
                    . "2. Nếu không có danh sách, tư vấn kiến thức chung về linh kiện.\n"
                    . "3. Từ chối lịch sự câu hỏi ngoài lĩnh vực máy tính.\n"
                    . "4. Trả lời tiếng Việt ngắn gọn, thân thiện.";

// 5. Gửi request đến Gemini
$api_url = "https://generativelanguage.googleapis.com/v1beta/models/" . GEMINI_MODEL . ":generateContent?key=" . GEMINI_API_KEY;

$postData = [
    'contents' => [
        ['parts' => [['text' => $systemInstruction . "\n\nKhách hỏi: " . $userQuestion]]]
    ],
    'generationConfig' => [
        'temperature' => 0.7,
        'maxOutputTokens' => 800
    ]
];

$ch = curl_init($api_url);
curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
curl_setopt($ch, CURLOPT_POST, true);
curl_setopt($ch, CURLOPT_POSTFIELDS, json_encode($postData));
curl_setopt($ch, CURLOPT_HTTPHEADER, ['Content-Type: application/json']);
curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, false);
curl_setopt($ch, CURLOPT_TIMEOUT, 30);

$response = curl_exec($ch);
$httpCode = curl_getinfo($ch, CURLINFO_HTTP_CODE);
curl_close($ch);

// 6. Xử lý phản hồi
if ($response === false) {
    echo json_encode(['success' => false, 'message' => 'Lỗi kết nối AI']);
} elseif ($httpCode !== 200) {
    echo json_encode(['success' => false, 'message' => 'API lỗi, vui lòng thử lại sau']);
} else {
    $result = json_decode($response, true);
    if (isset($result['candidates'][0]['content']['parts'][0]['text'])) {
        echo json_encode(['success' => true, 'reply' => trim($result['candidates'][0]['content']['parts'][0]['text'])]);
    } else {
        echo json_encode(['success' => false, 'message' => 'AI không xử lý được câu hỏi này']);
    }
}
?>
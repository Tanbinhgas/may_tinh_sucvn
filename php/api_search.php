<?php
header('Content-Type: application/json');

$host = 'localhost';
$user = 'root';
$password = '';
$database = 'tkl_computer';

$conn = new mysqli($host, $user, $password, $database);

if ($conn->connect_error) {
    echo json_encode(['error' => 'Kết nối DB thất bại']);
    exit;
}

$conn->set_charset("utf8");

$keyword = isset($_GET['keyword']) ? trim($_GET['keyword']) : '';

if (strlen($keyword) < 2) {
    echo json_encode([]);
    exit;
}

$searchTerm = "%$keyword%";
$stmt = $conn->prepare("
    SELECT id, name, price, brand, image_url 
    FROM products 
    WHERE name LIKE ? OR brand LIKE ? 
    ORDER BY name ASC 
    LIMIT 10
");

$stmt->bind_param("ss", $searchTerm, $searchTerm);
$stmt->execute();
$result = $stmt->get_result();

$products = [];
while ($row = $result->fetch_assoc()) {
    $products[] = [
        'id' => $row['id'],
        'name' => $row['name'],
        'price' => number_format($row['price'], 0, ',', '.') . 'đ',
        'brand' => $row['brand'] ?? 'Không có thương hiệu',
        'image' => $row['image_url'] ?? 'default.jpg'
    ];
}

echo json_encode($products);
$conn->close();
?>
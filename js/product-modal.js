/**
 * Product Modal with Specifications and Description
 * Handles displaying product specs and descriptions in modal
 */

// Sample Product Database with Specifications
const productDatabase = {
    'CPU Intel Core i9-13900K': {
        name: 'Intel Core i9-13900K',
        price: '15,800,000 VND',
        type: 'CPU',
        brand: 'Intel',
        series: 'Core i9 (13th Gen)',
        socket: 'LGA1700',
        cores: '24 Cores (8 P-cores + 16 E-cores)',
        threads: '32 Threads',
        baseFreq: '2.2 GHz',
        turboFreq: '5.80 GHz',
        tdp: '253W',
        cache: 'L3: 36MB',
        tech: 'Intel 7 (10nm)',
        size: '37.5mm x 37.5mm',
        weight: '0.065 kg',
        warranty: '36 tháng',
        origin: 'Nhập khẩu chính hãng',
        description: `Intel Core i9-13900K là một trong những bộ vi xử lý mạnh nhất hiện nay, 
            với 24 nhân (8 P-cores + 16 E-cores) và 32 luồng xử lý. 
            Với tần số boost lên đến 5.80 GHz, nó cung cấp hiệu suất siêu vượt trội cho gaming, 
            streaming, và các công việc xử lý đa luồng nặng.
            
            CPU này sử dụng kiến trúc Raptor Lake của Intel, mang lại cải tiến đáng kể về hiệu năng 
            so với thế hệ trước. Perfect cho những ai yêu cầu máy tính cấp cao nhất.`,
        highlights: [
            '✓ 24 cores mạnh mẽ cho multitasking',
            '✓ Boost clock 5.80 GHz',
            '✓ Hỗ trợ DDR5 và PCIe 5.0',
            '✓ Hiệu năng gaming tuyệt vời',
            '✓ Bảo hành chính hãng 36 tháng'
        ]
    },
    'Intel Core i7 13700K': {
        name: 'Intel Core i7 13700K',
        price: '13,900,000 VND',
        type: 'CPU',
        brand: 'Intel',
        series: 'Core i7 (13th Gen)',
        socket: 'LGA1700',
        cores: '16 Cores (8 P-cores + 8 E-cores)',
        threads: '24 Threads',
        baseFreq: '2.5 GHz',
        turboFreq: '5.4 GHz',
        tdp: '253W',
        cache: 'L3: 30MB',
        tech: 'Intel 7 (10nm)',
        size: '37.5mm x 37.5mm',
        weight: '0.065 kg',
        warranty: '36 tháng',
        origin: 'Nhập khẩu chính hãng',
        description: `Intel Core i7-13700K mang đến hiệu suất tuyệt vời cho gaming và content creation. 
            Với 16 cores và 24 threads, CPU này cung cấp sức mạnh đủ để xử lý các tác vụ phức tạp 
            trong khi vẫn đảm bảo hiệu năng gaming cao.
            
            Kiến trúc Raptor Lake cải tiến giúp tiết kiệm điện năng hơn so với thế hệ trước, 
            trong khi vẫn duy trì hiệu suất tuyệt vời.`,
        highlights: [
            '✓ 16 cores, 24 threads',
            '✓ Boost lên 5.4 GHz',
            '✓ Tiêu thụ điện năng hiệu quả',
            '✓ Tuyệt vời cho gaming 4K',
            '✓ Bảo hành 36 tháng'
        ]
    },
    'Intel Core i5-13500': {
        name: 'Intel Core i5-13500',
        price: '9,200,000 VND',
        type: 'CPU',
        brand: 'Intel',
        series: 'Core i5 (13th Gen)',
        socket: 'LGA1700',
        cores: '14 Cores (6 P-cores + 8 E-cores)',
        threads: '20 Threads',
        baseFreq: '1.8 GHz',
        turboFreq: '4.8 GHz',
        tdp: '65W',
        cache: 'L3: 24MB',
        tech: 'Intel 7 (10nm)',
        size: '37.5mm x 37.5mm',
        weight: '0.065 kg',
        warranty: '36 tháng',
        origin: 'Nhập khẩu chính hãng',
        description: `Intel Core i5-13500 là lựa chọn cân bằng hoàn hảo giữa hiệu năng và giá cả. 
            Với 14 cores và 20 threads, nó cung cấp đủ sức mạnh cho gaming, productivity, và streaming.
            
            Tiêu thụ điện năng thấp (65W TDP) giúp tiết kiệm chi phí điện và giảm nhiệt độ hệ thống.`,
        highlights: [
            '✓ 14 cores, 20 threads',
            '✓ TDP chỉ 65W - tiết kiệm điện',
            '✓ Giá tốt so với hiệu năng',
            '✓ Đủ mạnh cho mọi tác vụ',
            '✓ Bảo hành 36 tháng'
        ]
    },
    'Intel Core i3 13100': {
        name: 'Intel Core i3 13100',
        price: '3,200,000 VND',
        type: 'CPU',
        brand: 'Intel',
        series: 'Core i3 (13th Gen)',
        socket: 'LGA1700',
        cores: '4 Cores',
        threads: '8 Threads',
        baseFreq: '3.4 GHz',
        turboFreq: '4.5 GHz',
        tdp: '60W',
        cache: 'L3: 12MB',
        tech: 'Intel 7 (10nm)',
        size: '37.5mm x 37.5mm',
        weight: '0.065 kg',
        warranty: '36 tháng',
        origin: 'Nhập khẩu chính hãng',
        description: `Intel Core i3-13100 là CPU entry-level tuyệt vời cho người dùng thông thường 
            và những ai xây dựng PC với ngân sách hạn chế.
            
            Mặc dù có ít cores hơn, nhưng nó vẫn đủ cho công việc hàng ngày, học tập, và gaming ở cấp độ vừa phải.`,
        highlights: [
            '✓ Giá cực kỳ cạnh tranh',
            '✓ Tiêu thụ điện năng thấp',
            '✓ Đủ cho công việc văn phòng',
            '✓ Gaming casual hoàn toàn được',
            '✓ Bảo hành 36 tháng'
        ]
    }
};

/**
 * Hiển thị chi tiết sản phẩm trong modal
 * @param {string} productName - Tên sản phẩm cần hiển thị
 */
function showProductDetailsModal(productName) {
    const product = productDatabase[productName];
    
    if (!product) {
        console.warn('Product not found:', productName);
        return;
    }

    // Update specifications
    updateProductSpecs(product);
    
    // Update description
    updateProductDescription(product);
}

/**
 * Cập nhật bảng thông số kỹ thuật
 * @param {object} product - Đối tượng sản phẩm
 */
function updateProductSpecs(product) {
    document.getElementById('spec-type').textContent = product.type || '-';
    document.getElementById('spec-brand').textContent = product.brand || '-';
    document.getElementById('spec-series').textContent = product.series || '-';
    document.getElementById('spec-socket').textContent = product.socket || '-';
    document.getElementById('spec-cores').textContent = product.cores || '-';
    document.getElementById('spec-threads').textContent = product.threads || '-';
    document.getElementById('spec-base-freq').textContent = product.baseFreq || '-';
    document.getElementById('spec-turbo-freq').textContent = product.turboFreq || '-';
    document.getElementById('spec-tdp').textContent = product.tdp || '-';
    document.getElementById('spec-cache').textContent = product.cache || '-';
    document.getElementById('spec-tech').textContent = product.tech || '-';
    document.getElementById('spec-size').textContent = product.size || '-';
    document.getElementById('spec-weight').textContent = product.weight || '-';
    document.getElementById('spec-warranty').textContent = product.warranty || '-';
}

/**
 * Cập nhật mô tả sản phẩm
 * @param {object} product - Đối tượng sản phẩm
 */
function updateProductDescription(product) {
    // Update product name
    const productNameDesc = document.getElementById('product-name-desc');
    if (productNameDesc) {
        productNameDesc.textContent = product.name || 'Chi Tiết Sản Phẩm';
    }

    // Update description text
    const descText = document.getElementById('description-text');
    if (descText) {
        descText.innerHTML = product.description 
            ? '<p>' + product.description.split('\n').join('</p><p>') + '</p>'
            : '<p>Không có mô tả</p>';
    }

    // Update highlights
    const highlightsList = document.getElementById('highlights-list');
    if (highlightsList && product.highlights) {
        highlightsList.innerHTML = product.highlights
            .map(item => '<li>' + item + '</li>')
            .join('');
    }

    // Update additional info
    document.getElementById('info-condition').textContent = 'Mới 100%';
    document.getElementById('info-origin').textContent = product.origin || '-';
}

/**
 * Mở modal với thông số sản phẩm (khi click vào sản phẩm)
 * @param {element} element - Element được click
 */
function openProductModal(element) {
    const productName = element.getAttribute('data-product-name') || 
                       element.querySelector('.women h6')?.textContent || 
                       'Sản phẩm';
    
    // Tạo modal nếu chưa tồn tại
    if (!document.getElementById('productDetailsModal')) {
        createProductModal();
    }
    
    // Hiển thị chi tiết sản phẩm
    showProductDetailsModal(productName);
    
    // Mở modal
    $('#productDetailsModal').modal('show');
}

/**
 * Tạo modal cho chi tiết sản phẩm
 */
function createProductModal() {
    if (document.getElementById('productDetailsModal')) {
        return; // Modal đã tồn tại
    }

    const modalHTML = `
        <div class="modal fade" id="productDetailsModal" tabindex="-1" role="dialog">
            <div class="modal-dialog modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header" style="background: #039445; color: white; border: none;">
                        <h5 class="modal-title" style="color: white; font-weight: 700;">Chi Tiết Sản Phẩm</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close" style="color: white; opacity: 0.8;">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body" style="max-height: 70vh; overflow-y: auto;">
                        <!-- Product Specifications -->
                        <div id="modal-specs"></div>
                        
                        <!-- Product Description -->
                        <div id="modal-description"></div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Đóng</button>
                        <button type="button" class="btn btn-danger my-cart-btn">Thêm vào giỏ hàng</button>
                    </div>
                </div>
            </div>
        </div>
    `;

    document.body.insertAdjacentHTML('beforeend', modalHTML);
}

/**
 * Khởi tạo event listeners cho các sản phẩm
 */
function initProductModals() {
    // Click vào ảnh sản phẩm để xem chi tiết
    document.querySelectorAll('.offer-img').forEach(element => {
        element.addEventListener('click', function(e) {
            e.preventDefault();
            const productCard = this.closest('.col-m');
            const productName = productCard.querySelector('.women h6')?.textContent;
            if (productName && productDatabase[productName.trim()]) {
                openProductModal(productCard);
            }
        });
    });
}

// Khởi tạo khi DOM sẵn sàng
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', initProductModals);
} else {
    initProductModals();
}

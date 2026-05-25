-- ============================================================
--  insert_products.sql
--  Nhập toàn bộ sản phẩm từ website vào database
--  Chạy trong phpMyAdmin → database tkl_computer → tab SQL
-- ============================================================

USE tkl_computer;

-- ============================================================
-- CPU (category_id = 1)
-- ============================================================
INSERT INTO products (category_id, name, slug, price, stock, image_url, brand) VALUES
(1, 'CPU Intel Core i7 10700 Tray',               'cpu-intel-core-i7-10700-tray',              4990000, 10, '../images/product/cpu1.jpg',       'Intel'),
(1, 'CPU Intel Core i3 10100F Tray',              'cpu-intel-core-i3-10100f-tray',             1990000, 15, '../images/product/cpu2.jpg',       'Intel'),
(1, 'CPU Intel Pentium G5420',                    'cpu-intel-pentium-g5420',                   2990000, 8,  '../images/product/cpu3.jpg',       'Intel'),
(1, 'CPU cũ Intel Core i5 11400',                 'cpu-intel-core-i5-11400',                   3990000, 5,  '../images/product/cpu4.jpg',       'Intel'),
(1, 'CPU Intel Core i3-9100F',                    'cpu-intel-core-i3-9100f',                   3990000, 12, '../images/cpu/i39100f.webp',       'Intel'),
(1, 'Intel Core i5 12400F',                       'cpu-intel-core-i5-12400f',                  3990000, 20, '../images/cpu/i5.jpg',            'Intel'),
(1, 'CPU Intel Core i9-12900K',                   'cpu-intel-core-i9-12900k',                  3990000, 6,  '../images/cpu/i9.jpg',            'Intel'),
(1, 'Intel Core i9 10980XE',                      'cpu-intel-core-i9-10980xe',                 3990000, 4,  '../images/cpu/i910980xe.jpg',     'Intel'),
(1, 'Intel Core i9 12900K',                       'cpu-intel-core-i9-12900k-v2',               3990000, 7,  '../images/cpu/i912900k.webp',     'Intel'),
(1, 'CPU AMD RYZEN 3 3200G',                      'cpu-amd-ryzen-3-3200g',                     3990000, 10, '../images/cpu/ry3.jpg',           'AMD'),
(1, 'AMD Ryzen Threadripper Pro 5995WX',          'cpu-amd-ryzen-threadripper-pro-5995wx',     3990000, 3,  '../images/cpu/rythreadripper.jpg', 'AMD'),
(1, 'Intel Core i7 12700K',                       'cpu-intel-core-i7-12700k',                  3990000, 9,  '../images/cpu/i7.jpg',            'Intel');

-- ============================================================
-- GPU (category_id = 2)
-- ============================================================
INSERT INTO products (category_id, name, slug, price, stock, image_url, brand) VALUES
(2, 'ASUS TUF Gaming RTX 3060 Ti V2 OC 8GB',     'gpu-asus-tuf-rtx-3060ti-v2-oc-8gb',        10990000, 8,  '../images/product/gpu1.jpg',       'ASUS'),
(2, 'ZOTAC GAMING RTX 3060 Twin Edge OC',         'gpu-zotac-gaming-rtx-3060-twin-edge-oc',    6990000, 10, '../images/product/gpu2.jpg',       'ZOTAC'),
(2, 'ASUS TUF GTX 1660 Super-O6G GAMING',         'gpu-asus-tuf-gtx-1660-super-o6g-gaming',    5990000, 6,  '../images/product/gpu3.jpg',       'ASUS'),
(2, 'MSI GeForce RTX 4080 16GB GAMING X TRIO',    'gpu-msi-rtx-4080-16gb-gaming-x-trio',      12990000, 4,  '../images/product/gpu4.jpg',       'MSI');

-- ============================================================
-- RAM (category_id = 3)
-- ============================================================
INSERT INTO products (category_id, name, slug, price, stock, image_url, brand) VALUES
(3, 'RAM Corsair Vengeance LPX 8GB DDR4 3200',    'ram-corsair-vengeance-lpx-8gb-ddr4-3200',   1990000, 20, '../images/product/ram1.jpg',       'Corsair'),
(3, 'Ram Team Vulcan Z Gray DDR4-3200 32GB',       'ram-team-vulcan-z-gray-ddr4-3200-32gb',      990000, 15, '../images/product/ram2.jpg',       'Team'),
(3, 'Ram Gskill Led RGB SILVER DDR5 32GB Bus 7200','ram-gskill-led-rgb-silver-ddr5-32gb-7200',   2590000, 10, '../images/ram/ram1.webp',         'G.Skill'),
(3, 'Ram Gskill RIPJAWS S5 SILVER DDR5 32GB 5600', 'ram-gskill-ripjaws-s5-silver-ddr5-32gb-5600',2000000, 12, '../images/ram/ram2.webp',        'G.Skill');

-- ============================================================
-- SSD (category_id = 4)
-- ============================================================
INSERT INTO products (category_id, name, slug, price, stock, image_url, brand) VALUES
(4, 'SSD WD SN570 Blue 250GB M.2 PCIe NVMe 3x4',  'ssd-wd-sn570-blue-250gb-m2-pcie-nvme',      999000, 25, '../images/product/ssd1.jpg',       'WD'),
(4, 'SSD Lexar LNM610 PRO 500GB M.2 PCIe 3.0x4',  'ssd-lexar-lnm610-pro-500gb-m2-pcie',        899000, 20, '../images/product/ssd2.jpg',       'Lexar'),
(4, 'SSD Samsung 980 Pro 500GB PCIe NVMe 4.0x4',   'ssd-samsung-980-pro-500gb-pcie-nvme',       1999000, 15, '../images/product/ssd3.jpg',      'Samsung'),
(4, 'SSD Lexar NM620 512GB M.2 2280 PCIe 3.0x4',   'ssd-lexar-nm620-512gb-m2-pcie',            1199000, 18, '../images/product/ssd4.jpg',      'Lexar');

-- ============================================================
-- HDD (category_id = 5)
-- ============================================================
INSERT INTO products (category_id, name, slug, price, stock, image_url, brand) VALUES
(5, 'HDD WD Red Plus 2TB 3.5 inch 5400RPM',        'hdd-wd-red-plus-2tb-3-5-5400rpm',          2990000, 12, '../images/product/hdd1.jpg',       'WD'),
(5, 'HDD Seagate Ironwolf Pro 10TB 7200RPM',        'hdd-seagate-ironwolf-pro-10tb-7200rpm',    5990000, 6,  '../images/product/hdd2.jpg',       'Seagate'),
(5, 'HDD Seagate IronWolf 4TB 3.5 inch 5400RPM',   'hdd-seagate-ironwolf-4tb-3-5-5400rpm',     3990000, 10, '../images/product/hdd3.jpg',       'Seagate'),
(5, 'HDD Seagate Ironwolf Pro 20TB 7200RPM',        'hdd-seagate-ironwolf-pro-20tb-7200rpm',    6990000, 4,  '../images/product/hdd4.jpg',       'Seagate');

-- ============================================================
-- MAINBOARD (category_id = 6)
-- ============================================================
INSERT INTO products (category_id, name, slug, price, stock, image_url, brand) VALUES
(6, 'Mainboard ASUS PRIME H510M-D',                'mainboard-asus-prime-h510m-d',             1000000, 10, '../images/product/mb1.jpg',        'ASUS'),
(6, 'Mainboard Asrock H610M-HVS',                  'mainboard-asrock-h610m-hvs',               1100000, 8,  '../images/product/mb2.jpg',        'Asrock'),
(6, 'Mainboard GIGABYTE B560M GAMING HD',           'mainboard-gigabyte-b560m-gaming-hd',       1500000, 7,  '../images/product/mb3.jpg',        'GIGABYTE'),
(6, 'Mainboard ASUS PRIME B660M-A WIFI D4',         'mainboard-asus-prime-b660m-a-wifi-d4',     1900000, 9,  '../images/product/mb4.jpg',        'ASUS');

-- ============================================================
-- PC CASE (category_id = 7)
-- ============================================================
INSERT INTO products (category_id, name, slug, price, stock, image_url, brand) VALUES
(7, 'Case Xigmatek Scorpio 3F',                    'case-xigmatek-scorpio-3f',                  990000, 10, '../images/product/pccase1.jpg',    'Xigmatek'),
(7, 'Case Xigmatek FALCON',                        'case-xigmatek-falcon',                     1290000, 8,  '../images/product/pccase2.jpg',    'Xigmatek'),
(7, 'Case Cooler Master MasterBox Q300L',           'case-cooler-master-masterbox-q300l',       1490000, 6,  '../images/product/pccase3.jpg',    'Cooler Master'),
(7, 'Case Corsair 4000D Airflow',                   'case-corsair-4000d-airflow',               2490000, 5,  '../images/product/pccase4.jpg',    'Corsair');

-- ============================================================
-- PSU - NGUỒN (category_id = 8)
-- ============================================================
INSERT INTO products (category_id, name, slug, price, stock, image_url, brand) VALUES
(8, 'Nguồn Corsair CV550 550W 80 Plus Bronze',     'psu-corsair-cv550-550w-80plus-bronze',      990000, 15, '../images/product/sac1.jpg',       'Corsair'),
(8, 'Nguồn Corsair RM750x 750W 80 Plus Gold',      'psu-corsair-rm750x-750w-80plus-gold',      2490000, 10, '../images/product/sac2.jpg',       'Corsair'),
(8, 'Nguồn MSI MAG A650BN 650W 80 Plus Bronze',    'psu-msi-mag-a650bn-650w-80plus-bronze',    1290000, 12, '../images/product/sac3.jpg',       'MSI'),
(8, 'Nguồn Seasonic Focus GX-850 850W Gold',       'psu-seasonic-focus-gx-850-850w-gold',      3490000, 7,  '../images/product/sac4.jpg',       'Seasonic');

-- ============================================================
-- TẢN NHIỆT (category_id = 9)
-- ============================================================
INSERT INTO products (category_id, name, slug, price, stock, image_url, brand) VALUES
(9, 'Tản nhiệt DeepCool AG400 ARGB',               'tan-nhiet-deepcool-ag400-argb',              590000, 15, '../images/product/fan1.jpg',       'DeepCool'),
(9, 'Tản nhiệt Noctua NH-D15 chromax.black',       'tan-nhiet-noctua-nh-d15-chromax-black',    1990000, 6,  '../images/product/fan2.jpg',       'Noctua'),
(9, 'Tản nhiệt nước AIO Corsair H100i RGB Platinum','tan-nhiet-nuoc-corsair-h100i-rgb-platinum', 2990000, 8, '../images/product/fan3.jpg',       'Corsair'),
(9, 'Tản nhiệt nước AIO NZXT Kraken X63 RGB',      'tan-nhiet-nuoc-nzxt-kraken-x63-rgb',       3490000, 5,  '../images/product/fan4.jpg',       'NZXT');

-- ============================================================
-- MONITOR (category_id = 10)
-- ============================================================
INSERT INTO products (category_id, name, slug, price, stock, image_url, brand) VALUES
(10,'Màn hình ASUS VG279Q1A 27 inch FHD 165Hz IPS','man-hinh-asus-vg279q1a-27-fhd-165hz-ips',  4990000, 10, '../images/monitor/of8.png',       'ASUS'),
(10,'Màn hình Dell S2721DGF 27 inch QHD 165Hz IPS', 'man-hinh-dell-s2721dgf-27-qhd-165hz-ips',  7990000, 6, '../images/monitor/of9.png',       'Dell'),
(10,'Màn hình LG 27GP850-B 27 inch QHD 165Hz IPS',  'man-hinh-lg-27gp850-b-27-qhd-165hz-ips',  6990000, 8, '../images/monitor/of10.png',      'LG'),
(10,'Màn hình Samsung Odyssey G7 32 inch QHD 240Hz','man-hinh-samsung-odyssey-g7-32-qhd-240hz', 9990000, 4, '../images/monitor/of11.png',      'Samsung');

-- ============================================================
-- KEYBOARD (category_id = 11)
-- ============================================================
INSERT INTO products (category_id, name, slug, price, stock, image_url, brand) VALUES
(11,'Bàn phím cơ Keychron K2 V2 RGB Hot-swap',     'ban-phim-keychron-k2-v2-rgb-hot-swap',     1990000, 12, '../images/h1.jpg',                'Keychron'),
(11,'Bàn phím cơ AKKO 3087DS RGB',                  'ban-phim-akko-3087ds-rgb',                  990000, 15, '../images/h2.jpg',                'AKKO'),
(11,'Bàn phím cơ Logitech G Pro X TKL',             'ban-phim-logitech-g-pro-x-tkl',            2990000, 8,  '../images/h3.jpg',                'Logitech'),
(11,'Bàn phím cơ Razer BlackWidow V3 TKL',          'ban-phim-razer-blackwidow-v3-tkl',         2490000, 7,  '../images/h4.jpg',                'Razer');

-- ============================================================
-- MOUSE (category_id = 12)
-- ============================================================
INSERT INTO products (category_id, name, slug, price, stock, image_url, brand) VALUES
(12,'Chuột Logitech G Pro X Superlight 2',          'chuot-logitech-g-pro-x-superlight-2',      2990000, 10, '../images/m1.jpg',                'Logitech'),
(12,'Chuột Razer DeathAdder V3 HyperSpeed',         'chuot-razer-deathadder-v3-hyperspeed',     1990000, 12, '../images/m2.jpg',                'Razer'),
(12,'Chuột Zowie EC2-C',                            'chuot-zowie-ec2-c',                        1690000, 8,  '../images/m3.jpg',                'Zowie'),
(12,'Chuột SteelSeries Rival 650 Wireless',         'chuot-steelseries-rival-650-wireless',     2490000, 6,  '../images/m4.jpg',                'SteelSeries');

-- ============================================================
-- HEADSET (category_id = 13)
-- ============================================================
INSERT INTO products (category_id, name, slug, price, stock, image_url, brand) VALUES
(13,'Tai nghe Logitech G Pro X 2 LIGHTSPEED',       'tai-nghe-logitech-g-pro-x-2-lightspeed',   3990000, 8,  '../images/bp1.jpg',               'Logitech'),
(13,'Tai nghe SteelSeries Arctis Nova Pro',          'tai-nghe-steelseries-arctis-nova-pro',     5990000, 5,  '../images/bp2.jpg',               'SteelSeries'),
(13,'Tai nghe HyperX Cloud Alpha Wireless',          'tai-nghe-hyperx-cloud-alpha-wireless',     2990000, 10, '../images/bp3.jpg',               'HyperX'),
(13,'Tai nghe Razer BlackShark V2 Pro',              'tai-nghe-razer-blackshark-v2-pro',         2490000, 7,  '../images/bp4.jpg',               'Razer');

-- ============================================================
-- Kiểm tra kết quả
-- ============================================================
SELECT c.name AS danh_muc, COUNT(*) AS so_san_pham
FROM products p
JOIN categories c ON c.id = p.category_id
WHERE p.is_active = 1
GROUP BY c.name
ORDER BY c.id;

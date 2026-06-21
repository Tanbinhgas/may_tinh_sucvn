using may_tinh_sucvn.Models;

namespace may_tinh_sucvn.Data;

/// <summary>
/// Danh sách sản phẩm khởi tạo, trích từ website cũ (insert_products.sql) và ánh xạ sang
/// schema EF. CategoryId trùng Id danh mục đã seed trong AppDbContext (1..13).
/// Đường dẫn ảnh đã chuẩn hoá về web-root ("/images/...").
/// CreatedAt/UpdatedAt/IsActive dùng giá trị mặc định của entity Product.
/// </summary>
public static class SeedProducts
{
    public static List<Product> All() => new()
    {
        new() { CategoryId = 1, Name = "CPU Intel Core i7 10700 Tray", Slug = "cpu-intel-core-i7-10700-tray", Price = 4990000m, Stock = 10, ImageUrl = "/images/product/cpu1.jpg", Brand = "Intel" },
        new() { CategoryId = 1, Name = "CPU Intel Core i3 10100F Tray", Slug = "cpu-intel-core-i3-10100f-tray", Price = 1990000m, Stock = 15, ImageUrl = "/images/product/cpu2.jpg", Brand = "Intel" },
        new() { CategoryId = 1, Name = "CPU Intel Pentium G5420", Slug = "cpu-intel-pentium-g5420", Price = 2990000m, Stock = 8, ImageUrl = "/images/product/cpu3.jpg", Brand = "Intel" },
        new() { CategoryId = 1, Name = "CPU cũ Intel Core i5 11400", Slug = "cpu-intel-core-i5-11400", Price = 3990000m, Stock = 5, ImageUrl = "/images/product/cpu4.jpg", Brand = "Intel" },
        new() { CategoryId = 1, Name = "CPU Intel Core i3-9100F", Slug = "cpu-intel-core-i3-9100f", Price = 3990000m, Stock = 12, ImageUrl = "/images/cpu/i39100f.webp", Brand = "Intel" },
        new() { CategoryId = 1, Name = "Intel Core i5 12400F", Slug = "cpu-intel-core-i5-12400f", Price = 3990000m, Stock = 20, ImageUrl = "/images/cpu/i5.jpg", Brand = "Intel" },
        new() { CategoryId = 1, Name = "CPU Intel Core i9-12900K", Slug = "cpu-intel-core-i9-12900k", Price = 3990000m, Stock = 6, ImageUrl = "/images/cpu/i9.jpg", Brand = "Intel" },
        new() { CategoryId = 1, Name = "Intel Core i9 10980XE", Slug = "cpu-intel-core-i9-10980xe", Price = 3990000m, Stock = 4, ImageUrl = "/images/cpu/i910980xe.jpg", Brand = "Intel" },
        new() { CategoryId = 1, Name = "Intel Core i9 12900K", Slug = "cpu-intel-core-i9-12900k-v2", Price = 3990000m, Stock = 7, ImageUrl = "/images/cpu/i912900k.webp", Brand = "Intel" },
        new() { CategoryId = 1, Name = "CPU AMD RYZEN 3 3200G", Slug = "cpu-amd-ryzen-3-3200g", Price = 3990000m, Stock = 10, ImageUrl = "/images/cpu/ry3.jpg", Brand = "AMD" },
        new() { CategoryId = 1, Name = "AMD Ryzen Threadripper Pro 5995WX", Slug = "cpu-amd-ryzen-threadripper-pro-5995wx", Price = 3990000m, Stock = 3, ImageUrl = "/images/cpu/rythreadripper.jpg", Brand = "AMD" },
        new() { CategoryId = 1, Name = "Intel Core i7 12700K", Slug = "cpu-intel-core-i7-12700k", Price = 3990000m, Stock = 9, ImageUrl = "/images/cpu/i7.jpg", Brand = "Intel" },
        new() { CategoryId = 2, Name = "ASUS TUF Gaming RTX 3060 Ti V2 OC 8GB", Slug = "gpu-asus-tuf-rtx-3060ti-v2-oc-8gb", Price = 10990000m, Stock = 8, ImageUrl = "/images/product/gpu1.jpg", Brand = "ASUS" },
        new() { CategoryId = 2, Name = "ZOTAC GAMING RTX 3060 Twin Edge OC", Slug = "gpu-zotac-gaming-rtx-3060-twin-edge-oc", Price = 6990000m, Stock = 10, ImageUrl = "/images/product/gpu2.jpg", Brand = "ZOTAC" },
        new() { CategoryId = 2, Name = "ASUS TUF GTX 1660 Super-O6G GAMING", Slug = "gpu-asus-tuf-gtx-1660-super-o6g-gaming", Price = 5990000m, Stock = 6, ImageUrl = "/images/product/gpu3.jpg", Brand = "ASUS" },
        new() { CategoryId = 2, Name = "MSI GeForce RTX 4080 16GB GAMING X TRIO", Slug = "gpu-msi-rtx-4080-16gb-gaming-x-trio", Price = 12990000m, Stock = 4, ImageUrl = "/images/product/gpu4.jpg", Brand = "MSI" },
        new() { CategoryId = 3, Name = "RAM Corsair Vengeance LPX 8GB DDR4 3200", Slug = "ram-corsair-vengeance-lpx-8gb-ddr4-3200", Price = 1990000m, Stock = 20, ImageUrl = "/images/product/ram1.jpg", Brand = "Corsair" },
        new() { CategoryId = 3, Name = "Ram Team Vulcan Z Gray DDR4-3200 32GB", Slug = "ram-team-vulcan-z-gray-ddr4-3200-32gb", Price = 990000m, Stock = 15, ImageUrl = "/images/product/ram2.jpg", Brand = "Team" },
        new() { CategoryId = 3, Name = "Ram Gskill Led RGB SILVER DDR5 32GB Bus 7200", Slug = "ram-gskill-led-rgb-silver-ddr5-32gb-7200", Price = 2590000m, Stock = 10, ImageUrl = "/images/ram/ram1.webp", Brand = "G.Skill" },
        new() { CategoryId = 3, Name = "Ram Gskill RIPJAWS S5 SILVER DDR5 32GB 5600", Slug = "ram-gskill-ripjaws-s5-silver-ddr5-32gb-5600", Price = 2000000m, Stock = 12, ImageUrl = "/images/ram/ram2.webp", Brand = "G.Skill" },
        new() { CategoryId = 4, Name = "SSD WD SN570 Blue 250GB M.2 PCIe NVMe 3x4", Slug = "ssd-wd-sn570-blue-250gb-m2-pcie-nvme", Price = 999000m, Stock = 25, ImageUrl = "/images/product/ssd1.jpg", Brand = "WD" },
        new() { CategoryId = 4, Name = "SSD Lexar LNM610 PRO 500GB M.2 PCIe 3.0x4", Slug = "ssd-lexar-lnm610-pro-500gb-m2-pcie", Price = 899000m, Stock = 20, ImageUrl = "/images/product/ssd2.jpg", Brand = "Lexar" },
        new() { CategoryId = 4, Name = "SSD Samsung 980 Pro 500GB PCIe NVMe 4.0x4", Slug = "ssd-samsung-980-pro-500gb-pcie-nvme", Price = 1999000m, Stock = 15, ImageUrl = "/images/product/ssd3.jpg", Brand = "Samsung" },
        new() { CategoryId = 4, Name = "SSD Lexar NM620 512GB M.2 2280 PCIe 3.0x4", Slug = "ssd-lexar-nm620-512gb-m2-pcie", Price = 1199000m, Stock = 18, ImageUrl = "/images/product/ssd4.jpg", Brand = "Lexar" },
        new() { CategoryId = 5, Name = "HDD WD Red Plus 2TB 3.5 inch 5400RPM", Slug = "hdd-wd-red-plus-2tb-3-5-5400rpm", Price = 2990000m, Stock = 12, ImageUrl = "/images/product/hdd1.jpg", Brand = "WD" },
        new() { CategoryId = 5, Name = "HDD Seagate Ironwolf Pro 10TB 7200RPM", Slug = "hdd-seagate-ironwolf-pro-10tb-7200rpm", Price = 5990000m, Stock = 6, ImageUrl = "/images/product/hdd2.jpg", Brand = "Seagate" },
        new() { CategoryId = 5, Name = "HDD Seagate IronWolf 4TB 3.5 inch 5400RPM", Slug = "hdd-seagate-ironwolf-4tb-3-5-5400rpm", Price = 3990000m, Stock = 10, ImageUrl = "/images/product/hdd3.jpg", Brand = "Seagate" },
        new() { CategoryId = 5, Name = "HDD Seagate Ironwolf Pro 20TB 7200RPM", Slug = "hdd-seagate-ironwolf-pro-20tb-7200rpm", Price = 6990000m, Stock = 4, ImageUrl = "/images/product/hdd4.jpg", Brand = "Seagate" },
        new() { CategoryId = 6, Name = "Mainboard ASUS PRIME H510M-D", Slug = "mainboard-asus-prime-h510m-d", Price = 1000000m, Stock = 10, ImageUrl = "/images/product/mb1.jpg", Brand = "ASUS" },
        new() { CategoryId = 6, Name = "Mainboard Asrock H610M-HVS", Slug = "mainboard-asrock-h610m-hvs", Price = 1100000m, Stock = 8, ImageUrl = "/images/product/mb2.jpg", Brand = "Asrock" },
        new() { CategoryId = 6, Name = "Mainboard GIGABYTE B560M GAMING HD", Slug = "mainboard-gigabyte-b560m-gaming-hd", Price = 1500000m, Stock = 7, ImageUrl = "/images/product/mb3.jpg", Brand = "GIGABYTE" },
        new() { CategoryId = 6, Name = "Mainboard ASUS PRIME B660M-A WIFI D4", Slug = "mainboard-asus-prime-b660m-a-wifi-d4", Price = 1900000m, Stock = 9, ImageUrl = "/images/product/mb4.jpg", Brand = "ASUS" },
        new() { CategoryId = 7, Name = "Case Xigmatek Scorpio 3F", Slug = "case-xigmatek-scorpio-3f", Price = 990000m, Stock = 10, ImageUrl = "/images/product/pccase1.jpg", Brand = "Xigmatek" },
        new() { CategoryId = 7, Name = "Case Xigmatek FALCON", Slug = "case-xigmatek-falcon", Price = 1290000m, Stock = 8, ImageUrl = "/images/product/pccase2.jpg", Brand = "Xigmatek" },
        new() { CategoryId = 7, Name = "Case Cooler Master MasterBox Q300L", Slug = "case-cooler-master-masterbox-q300l", Price = 1490000m, Stock = 6, ImageUrl = "/images/product/pccase3.jpg", Brand = "Cooler Master" },
        new() { CategoryId = 7, Name = "Case Corsair 4000D Airflow", Slug = "case-corsair-4000d-airflow", Price = 2490000m, Stock = 5, ImageUrl = "/images/product/pccase4.jpg", Brand = "Corsair" },
        new() { CategoryId = 8, Name = "Nguồn Corsair CV550 550W 80 Plus Bronze", Slug = "psu-corsair-cv550-550w-80plus-bronze", Price = 990000m, Stock = 15, ImageUrl = "/images/product/sac1.jpg", Brand = "Corsair" },
        new() { CategoryId = 8, Name = "Nguồn Corsair RM750x 750W 80 Plus Gold", Slug = "psu-corsair-rm750x-750w-80plus-gold", Price = 2490000m, Stock = 10, ImageUrl = "/images/product/sac2.jpg", Brand = "Corsair" },
        new() { CategoryId = 8, Name = "Nguồn MSI MAG A650BN 650W 80 Plus Bronze", Slug = "psu-msi-mag-a650bn-650w-80plus-bronze", Price = 1290000m, Stock = 12, ImageUrl = "/images/product/sac3.jpg", Brand = "MSI" },
        new() { CategoryId = 8, Name = "Nguồn Seasonic Focus GX-850 850W Gold", Slug = "psu-seasonic-focus-gx-850-850w-gold", Price = 3490000m, Stock = 7, ImageUrl = "/images/product/sac4.jpg", Brand = "Seasonic" },
        new() { CategoryId = 9, Name = "Tản nhiệt DeepCool AG400 ARGB", Slug = "tan-nhiet-deepcool-ag400-argb", Price = 590000m, Stock = 15, ImageUrl = "/images/product/fan1.jpg", Brand = "DeepCool" },
        new() { CategoryId = 9, Name = "Tản nhiệt Noctua NH-D15 chromax.black", Slug = "tan-nhiet-noctua-nh-d15-chromax-black", Price = 1990000m, Stock = 6, ImageUrl = "/images/product/fan2.jpg", Brand = "Noctua" },
        new() { CategoryId = 9, Name = "Tản nhiệt nước AIO Corsair H100i RGB Platinum", Slug = "tan-nhiet-nuoc-corsair-h100i-rgb-platinum", Price = 2990000m, Stock = 8, ImageUrl = "/images/product/fan3.jpg", Brand = "Corsair" },
        new() { CategoryId = 9, Name = "Tản nhiệt nước AIO NZXT Kraken X63 RGB", Slug = "tan-nhiet-nuoc-nzxt-kraken-x63-rgb", Price = 3490000m, Stock = 5, ImageUrl = "/images/product/fan4.jpg", Brand = "NZXT" },
        new() { CategoryId = 10, Name = "Màn hình ASUS VG279Q1A 27 inch FHD 165Hz IPS", Slug = "man-hinh-asus-vg279q1a-27-fhd-165hz-ips", Price = 4990000m, Stock = 10, ImageUrl = "/images/monitor/of8.png", Brand = "ASUS" },
        new() { CategoryId = 10, Name = "Màn hình Dell S2721DGF 27 inch QHD 165Hz IPS", Slug = "man-hinh-dell-s2721dgf-27-qhd-165hz-ips", Price = 7990000m, Stock = 6, ImageUrl = "/images/monitor/of9.png", Brand = "Dell" },
        new() { CategoryId = 10, Name = "Màn hình LG 27GP850-B 27 inch QHD 165Hz IPS", Slug = "man-hinh-lg-27gp850-b-27-qhd-165hz-ips", Price = 6990000m, Stock = 8, ImageUrl = "/images/monitor/of10.png", Brand = "LG" },
        new() { CategoryId = 10, Name = "Màn hình Samsung Odyssey G7 32 inch QHD 240Hz", Slug = "man-hinh-samsung-odyssey-g7-32-qhd-240hz", Price = 9990000m, Stock = 4, ImageUrl = "/images/monitor/of11.png", Brand = "Samsung" },
        new() { CategoryId = 11, Name = "Bàn phím cơ Keychron K2 V2 RGB Hot-swap", Slug = "ban-phim-keychron-k2-v2-rgb-hot-swap", Price = 1990000m, Stock = 12, ImageUrl = "/images/h1.jpg", Brand = "Keychron" },
        new() { CategoryId = 11, Name = "Bàn phím cơ AKKO 3087DS RGB", Slug = "ban-phim-akko-3087ds-rgb", Price = 990000m, Stock = 15, ImageUrl = "/images/h2.jpg", Brand = "AKKO" },
        new() { CategoryId = 11, Name = "Bàn phím cơ Logitech G Pro X TKL", Slug = "ban-phim-logitech-g-pro-x-tkl", Price = 2990000m, Stock = 8, ImageUrl = "/images/h3.jpg", Brand = "Logitech" },
        new() { CategoryId = 11, Name = "Bàn phím cơ Razer BlackWidow V3 TKL", Slug = "ban-phim-razer-blackwidow-v3-tkl", Price = 2490000m, Stock = 7, ImageUrl = "/images/h4.jpg", Brand = "Razer" },
        new() { CategoryId = 12, Name = "Chuột Logitech G Pro X Superlight 2", Slug = "chuot-logitech-g-pro-x-superlight-2", Price = 2990000m, Stock = 10, ImageUrl = "/images/m1.jpg", Brand = "Logitech" },
        new() { CategoryId = 12, Name = "Chuột Razer DeathAdder V3 HyperSpeed", Slug = "chuot-razer-deathadder-v3-hyperspeed", Price = 1990000m, Stock = 12, ImageUrl = "/images/m2.jpg", Brand = "Razer" },
        new() { CategoryId = 12, Name = "Chuột Zowie EC2-C", Slug = "chuot-zowie-ec2-c", Price = 1690000m, Stock = 8, ImageUrl = "/images/m3.jpg", Brand = "Zowie" },
        new() { CategoryId = 12, Name = "Chuột SteelSeries Rival 650 Wireless", Slug = "chuot-steelseries-rival-650-wireless", Price = 2490000m, Stock = 6, ImageUrl = "/images/m4.jpg", Brand = "SteelSeries" },
        new() { CategoryId = 13, Name = "Tai nghe Logitech G Pro X 2 LIGHTSPEED", Slug = "tai-nghe-logitech-g-pro-x-2-lightspeed", Price = 3990000m, Stock = 8, ImageUrl = "/images/bp1.jpg", Brand = "Logitech" },
        new() { CategoryId = 13, Name = "Tai nghe SteelSeries Arctis Nova Pro", Slug = "tai-nghe-steelseries-arctis-nova-pro", Price = 5990000m, Stock = 5, ImageUrl = "/images/bp2.jpg", Brand = "SteelSeries" },
        new() { CategoryId = 13, Name = "Tai nghe HyperX Cloud Alpha Wireless", Slug = "tai-nghe-hyperx-cloud-alpha-wireless", Price = 2990000m, Stock = 10, ImageUrl = "/images/bp3.jpg", Brand = "HyperX" },
        new() { CategoryId = 13, Name = "Tai nghe Razer BlackShark V2 Pro", Slug = "tai-nghe-razer-blackshark-v2-pro", Price = 2490000m, Stock = 7, ImageUrl = "/images/bp4.jpg", Brand = "Razer" },
    };
}

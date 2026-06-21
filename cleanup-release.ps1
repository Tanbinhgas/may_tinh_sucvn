# Dọn dẹp trước khi phát hành: xoá file trùng entry point, các trang category tĩnh
# đã được thay bằng trang động /category/{slug}, và code-behind trang chủ thừa.
# Chạy tại thư mục gốc dự án:  powershell -ExecutionPolicy Bypass -File .\cleanup-release.ps1

$ErrorActionPreference = 'Stop'

# 1) Entry point trùng (gây lỗi build CS8802)
Remove-Item 'Data\Program.cs' -ErrorAction SilentlyContinue

# 2) Code-behind trang chủ thừa (Index.cshtml hiện là HTML tĩnh, không model)
Remove-Item 'Pages\Index.cshtml.cs' -ErrorAction SilentlyContinue

# 3) 13 trang category tĩnh đã thay bằng trang động
$pages = 'cpu','gpu','ram','ssd','hdd','mainboard','PCcase','PowerSupply',
         'fan_liquidcooling','minitor','keyboard','mouse','headset'
foreach ($p in $pages) {
    Remove-Item "Pages\$p.cshtml"    -ErrorAction SilentlyContinue
    Remove-Item "Pages\$p.cshtml.cs" -ErrorAction SilentlyContinue
}

# 4) Gỡ .claude khỏi theo dõi git (nếu đã lỡ commit)
git rm -r --cached .claude 2>$null

Write-Host 'Done. Da don dep xong. Hay chay: dotnet build'

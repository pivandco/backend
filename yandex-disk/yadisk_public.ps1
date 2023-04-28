# PowerShell 5 compatible - not working with cyrillic cymbols

$headers = @{
    'Content-Type' = 'application/json'
    'Accept' = 'application/json'
    'Authorization' = 'OAuth your_auth_key' # write your auth key (https://yandex.ru/dev/oauth/)
}

$body = @"
{"custom_properties": {"sample":"text", "1":"2"}}
"@

Write-Output "Metainformation of disk"
Invoke-RestMethod -Method Get -Headers $headers -Uri https://cloud-api.yandex.net/v1/disk/
Read-Host -Prompt "Press Enter to continue"

Write-Output "Metainformation of file"
Invoke-RestMethod -Method Get -Headers $headers -Uri https://cloud-api.yandex.net/v1/disk/resources?path=backend-2%2Fsimple.txt
Read-Host -Prompt "Press Enter to continue"

Write-Output "Creating a foolder"
Invoke-RestMethod -Method Put -Headers $headers -Uri https://cloud-api.yandex.net/v1/disk/resources?path=backend-2%2Ffoolder
Read-Host -Prompt "Press Enter to continue"

Write-Output "Copy a file"
Invoke-RestMethod -Method Post -Headers $headers -Uri "https://cloud-api.yandex.net/v1/disk/resources/copy?from=backend-2%2Fsimple.txt&path=backend-2%2Ffoolder%2Fsimple.txt&overwrite=true"
Read-Host -Prompt "Press Enter to continue"

Write-Output "User arguments"
Invoke-RestMethod -Method Patch -Headers $headers -Body $body -Uri https://cloud-api.yandex.net/v1/disk/resources?path=backend-2%2Fsimple.txt
Read-Host -Prompt "Press Enter to continue"

# Extended
$confirmation = Read-Host "Write Y to delete a foolder"
if ($confirmation -eq 'y') {
     Invoke-RestMethod -Method Delete -Headers $headers -Uri "https://cloud-api.yandex.net/v1/disk/resources?path=backend-2%2Ffoolder&permanently=true"
}
Read-Host -Prompt "Press Enter to continue"
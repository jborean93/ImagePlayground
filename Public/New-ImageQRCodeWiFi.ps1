﻿function New-ImageQRCodeWiFi {
    [alias('New-QRCodeWiFi')]
    [cmdletBinding()]
    param(
        [Parameter(Mandatory)][string] $SSID,
        [Parameter(Mandatory)][string] $Password,
        [Parameter(Mandatory)][string] $FilePath,
        [switch] $Show
    )
    if (-not $FilePath) {
        $FilePath = [System.IO.Path]::Combine([System.IO.Path]::GetTempPath(), "$($([System.IO.Path]::GetRandomFileName()).Split('.')[0]).png")
        Write-Warning -Message "New-ImageQRCodeWiFi - No file path specified, saving to $FilePath"
    }
    try {
        [ImagePlayground.QrCode]::GenerateWiFi($ssid, $password, $FilePath, $false)

        if ($Show) {
            Invoke-Item -LiteralPath $FilePath
        }
    } catch {
        if ($PSBoundParameters.ErrorAction -eq 'Stop') {
            throw
        } else {
            Write-Warning -Message "New-ImageQRCodeWiFi - Error creating image $($_.Exception.Message)"
        }
    }
}
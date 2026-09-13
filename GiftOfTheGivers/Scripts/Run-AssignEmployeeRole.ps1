param(
	[string]$Server = 'localhost',
	[string]$Database = 'GiftOfTheGivers',
	[string]$UserEmail = 'test@example.com'
)

# Usage: .\Run-AssignEmployeeRole.ps1 -Server "(localdb)\\MSSQLLocalDB" -Database "GiftOfTheGivers" -UserEmail "test@example.com"

$scriptPath = Join-Path $PSScriptRoot 'AssignEmployeeRole.sql'

if (-not (Test-Path $scriptPath)) {
	Write-Error "SQL script not found: $scriptPath"
	exit 1
}

# Read SQL and replace the placeholder email
$sql = Get-Content $scriptPath -Raw
$sql = $sql -replace "test@example.com", $UserEmail

try {
	if (Get-Command Invoke-Sqlcmd -ErrorAction SilentlyContinue) {
		Invoke-Sqlcmd -ServerInstance $Server -Database $Database -Query $sql -ErrorAction Stop
	}
	else {
		# Fall back to sqlcmd.exe if available
		$tmpFile = [System.IO.Path]::GetTempFileName() + '.sql'
		Set-Content -Path $tmpFile -Value $sql -Encoding UTF8
		$args = "-S `"$Server`" -d `"$Database`" -i `"$tmpFile`""
		$proc = Start-Process -FilePath sqlcmd -ArgumentList $args -NoNewWindow -Wait -PassThru -ErrorAction Stop
		Remove-Item $tmpFile -ErrorAction SilentlyContinue
	}
	Write-Host "Script executed. Verify in DB or check output above."
}
catch {
	Write-Error "Failed to run SQL script: $_"
	exit 1
}

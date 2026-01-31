
Twilio recovery code: O3JgDuh1Sktd1QlClGVPY7hsjEoVC4mX1uaJNALE
# Get-ChildItem  -Recurse .\| Where-Object { $_.PSIsContainer }| Where-Object {$_.Name -match "^(bin|obj)$" }| Where-Object {$_.FullName -notmatch ".+node_modules.+"}| Remove-Item -Force -Recurse
# find ./ -regex "^(?!.+\/node_modules\/.*)((.+\/bin\/.*)|(.+\/obj\/.*))$" -type d -exec rm -rf {} \;
#docker compose run --rm  certbot certonly --webroot --webroot-path /var/www/certbot/ -d example.org
#docker compose run --rm certbot renew
#sudo cetbot certonly -d dev.dishnfork.com -d www.dev.dishnfork.com
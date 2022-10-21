ngrok authtoken 4D2ABehJoZ4TFvshVhFdb_4hubtMxe1N1rcZe4Agms3

start "C:\Program Files (x86)\Google\Chrome\Application\chrome.exe" http://eduardolucasmaia.ngrok.io

ngrok.exe http -subdomain=eduardolucasmaia -host-header=rewrite localhost:65033

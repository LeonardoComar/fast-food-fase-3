# Definir a URL do endpoint de API para teste funcional
API_URL="https://www.google.com.br"

# Dados de exemplo para enviar no teste funcional
data='{"param1":"valor1","param2":"valor2"}'

# Chamar o endpoint da API com os dados de exemplo e verificar a resposta
response=$(curl --write-out "%{http_code}" --silent --output /dev/null -X POST -H "Content-Type: application/json" -d "$data" "$API_URL")

# Se a resposta for 200, o teste funcional passou
if [ "$response" -eq 200 ]; then
  echo "Teste funcional passou: O endpoint da Api está respondendo corretamente."
  exit 0
else
  echo "Teste funcional falhou: O endpoint da Api não está respondendo corretamente."
  exit 1
fi
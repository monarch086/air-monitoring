#include <Adafruit_BMP085.h>
#include <Adafruit_SHT31.h>
#include <ESP8266WiFi.h>
#include <ESP8266HTTPClient.h>
#include <Arduino_JSON.h>     //https://github.com/arduino-libraries/Arduino_JSON

#define seaLevelPressure_hPa 1013.25

Adafruit_BMP085 bmp;
Adafruit_SHT31 sht31 = Adafruit_SHT31();

const char* ssid = "ssid"; // Wi-Fi network id
const char* password = "password"; // Wi-Fi network password

const char* host = "https://65qbkztuk2.execute-api.eu-central-1.amazonaws.com/dev/measurements";
const int httpsPort = 443;

const String deviceId = "S4D-12";
const String query = "?id=" + deviceId;
const String url = host + query;

void setup() {
  Serial.begin(115200);
  delay(10);

  if (!bmp.begin() || !sht31.begin(0x44)) {
    Serial.println("Sensors not found. CHECK CIRCUIT!");
    while (1) {}
  }

  WiFi.begin(ssid, password);
}

void loop() {
  printData();
  
  sendData();
  
  Serial.print("Sleep for 10 min...\n");
  delay(600000); // 10 min
}

void printData() {
  float t = sht31.readTemperature();
  float h = sht31.readHumidity();

  Serial.print("[BMP085] Temperature = ");
  Serial.print(bmp.readTemperature());
  Serial.println(" *C");

  Serial.print("[BMP085] Pressure = ");
  Serial.print(bmp.readPressure());
  Serial.println(" Pa");

  Serial.print("[BMP085] Pressure at sealevel (calculated) = ");
  Serial.print(bmp.readSealevelPressure());
  Serial.println(" Pa");

  Serial.print("[BMP085] Altitude = ");
  Serial.print(bmp.readAltitude());
  Serial.println(" meters");

  Serial.print("[BMP085] Real altitude = ");
  Serial.print(bmp.readAltitude(seaLevelPressure_hPa * 100));
  Serial.println(" meters");

  if (!isnan(t)) {
    Serial.print("[SHT31] Temp *C = ");
    Serial.println(t);
  } else {
    t = 0.0;
    Serial.println("[SHT31] Failed to read temperature");
  }

  if (!isnan(h)) {
    Serial.print("[SHT31] Hum. % = ");
    Serial.println(h);
  } else {
    h = 0.0;
    Serial.println("[SHT31] Failed to read humidity");
  }

  Serial.println();
}

void sendData() {
    unsigned short count = 0;

  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
    count++;

    if (count >= 30) {
      wifiRestart();
      count = 0;
    }
  }

  Serial.println("");
  Serial.println("WiFi connected");
  Serial.println("IP address: ");
  Serial.println(WiFi.localIP());

  HTTPClient http;
  WiFiClientSecure httpsClient;

  Serial.println(url);

  httpsClient.setInsecure();

  delay(1000);

  httpsClient.connect(host, httpsPort);

  Serial.print("[HTTP] begin...\n");
  http.begin(httpsClient, url);
  http.addHeader("Content-Type", "application/json");

  String body = createBody();
  Serial.printf("Body: %s\n", body.c_str());

  int httpCode = http.POST(body);
  if (httpCode > 0) {
    http.writeToStream(&Serial);

    Serial.printf("[HTTP] ... code: %d\n", httpCode);

    if (httpCode >= 200 and httpCode <= 299) {
      String payload = http.getString();
      Serial.printf("Payload: %s\n", payload);
    }
  } else {
    Serial.printf("[HTTP] ... failed, error: %s\n", http.errorToString(httpCode).c_str());
    String payload = http.getString();
    Serial.printf("Payload: %s\n", payload);
  }

  Serial.print("[HTTP] end...\n");
  http.end();
}

void wifiRestart() {
  Serial.println("Restarting Wi-Fi: turning WiFi off...");
  WiFi.mode(WIFI_OFF);
  Serial.println("Sleeping for 10 seconds...");
  delay(10000); // 10 sec
  Serial.println("Trying to connect to WiFi...");
  WiFi.mode(WIFI_STA);

  WiFi.begin(ssid, password);
}

String createBody() {
  JSONVar requestBody;

  requestBody["BMP085"]["Temperature"] = String(bmp.readTemperature());
  requestBody["BMP085"]["Pressure"] = bmp.readPressure();

  requestBody["SHT31"]["Temperature"] = String(sht31.readTemperature());
  requestBody["SHT31"]["Humidity"] = String(sht31.readHumidity());

  return JSON.stringify(requestBody);
}

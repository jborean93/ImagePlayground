﻿using System;
using System.Drawing.Imaging;
using System.IO;
using BarcodeReader.ImageSharp;
using QRCoder;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ImagePlayground {
    public class QrCode {
        public static void Generate(string content, string filePath, bool transparent = false, QRCodeGenerator.ECCLevel eccLevel = QRCodeGenerator.ECCLevel.Q) {

            FileInfo fileInfo = new FileInfo(filePath);

            using (QRCodeGenerator qrGenerator = new QRCodeGenerator()) {
                using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(content, eccLevel)) {
                    using (QRCoder.QRCode qrCode = new QRCoder.QRCode(qrCodeData)) {
                        using (var qrCodeImage = qrCode.GetGraphic(20)) {
                            if (transparent) {
                                //qrCodeImage.MakeTransparent();
                            }
                            // this uses QRCoder
                            //ImageFormat imageFormatDetected;
                            //if (fileInfo.Extension == ".png") {
                            //    imageFormatDetected = ImageFormat.Png;
                            //} else if (fileInfo.Extension == ".jpg" || fileInfo.Extension == ".jpeg") {
                            //    imageFormatDetected = ImageFormat.Jpeg;
                            //} else if (fileInfo.Extension == ".ico") {
                            //    imageFormatDetected = ImageFormat.Icon;
                            //} else {
                            //    throw new UnknownImageFormatException("Image format not supported. Feel free to open an issue/fix it.");
                            //}
                            //qrCodeImage.Save(filePath, imageFormatDetected);

                            //this uses QRCoder.ImageSharp
                            if (fileInfo.Extension == ".png") {
                                qrCodeImage.SaveAsPng(filePath);
                            } else if (fileInfo.Extension == ".jpg" || fileInfo.Extension == ".jpeg") {
                                qrCodeImage.SaveAsJpeg(filePath);
                            } else if (fileInfo.Extension == ".ico") {
                                qrCodeImage.SaveAsWebp(filePath);
                            } else {
                                throw new UnknownImageFormatException("Image format not supported. Feel free to open an issue/fix it.");
                            }
                        }
                    }
                }
            }
        }
        public static void GenerateWiFi(string ssid, string password, string filePath, bool transparent = false) {
            PayloadGenerator.WiFi generator = new PayloadGenerator.WiFi(ssid, password, PayloadGenerator.WiFi.Authentication.WPA);
            Generate(generator.ToString(), filePath, transparent);
        }

        public static void GenerateWhatsAppMessage(string message, string filePath, bool transparent = false) {
            PayloadGenerator.WhatsAppMessage generator = new PayloadGenerator.WhatsAppMessage(message);
            Generate(generator.ToString(), filePath, transparent);
        }
        public static void GenerateUrl(string url, string filePath, bool transparent = false) {
            PayloadGenerator.Url generator = new PayloadGenerator.Url(url);
            Generate(generator.ToString(), filePath, transparent);
        }
        public static void GenerateBookmark(string bookmarkUrl, string bookmarkName, string filePath, bool transparent = false) {
            PayloadGenerator.Bookmark generator = new PayloadGenerator.Bookmark(bookmarkUrl, bookmarkName);
            Generate(generator.ToString(), filePath, transparent);
        }

        public static void GenerateCalendarEvent(string calendarEntry, string calendarMessage, string calendarGeoLocation, DateTime calendarFrom, DateTime calendarTo, string filePath, bool allDayEvent, PayloadGenerator.CalendarEvent.EventEncoding calendarEventEncoding = PayloadGenerator.CalendarEvent.EventEncoding.iCalComplete, bool transparent = false) {
            PayloadGenerator.CalendarEvent generator = new PayloadGenerator.CalendarEvent(calendarEntry, calendarMessage, calendarGeoLocation, calendarFrom, calendarTo, allDayEvent, calendarEventEncoding);
            Generate(generator.ToString(), filePath, transparent);
        }

        public static void GenerateContact(string filePath, PayloadGenerator.ContactData.ContactOutputType outputType, string firstname, string lastname, string nickname = null, string phone = null, string mobilePhone = null, string workPhone = null, string email = null, DateTime? birthday = null, string website = null, string street = null, string houseNumber = null, string city = null, string zipCode = null, string country = null, string note = null, string stateRegion = null, PayloadGenerator.ContactData.AddressOrder addressOrder = PayloadGenerator.ContactData.AddressOrder.Default, string org = null, string orgTitle = null, bool transparent = false) {
            PayloadGenerator.ContactData generator = new PayloadGenerator.ContactData(outputType, firstname, lastname, nickname, phone, mobilePhone, workPhone, email, birthday, website, street, houseNumber, city, zipCode, country, note, stateRegion, addressOrder, org, orgTitle);
            Generate(generator.ToString(), filePath, transparent);
        }

        public static BarcodeResult<Rgba32> Read(string filePath) {
            Image<Rgba32> barcodeImage = Image.Load<Rgba32>(filePath);
            BarcodeReader<Rgba32> reader = new BarcodeReader<Rgba32>(types: ZXing.BarcodeFormat.QR_CODE);
            var response = reader.Decode(barcodeImage);
            return response;
        }
    }
}
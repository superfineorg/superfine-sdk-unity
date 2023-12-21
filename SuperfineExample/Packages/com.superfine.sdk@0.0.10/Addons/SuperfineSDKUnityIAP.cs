using UnityEngine;
using UnityEngine.Purchasing;
using System;
using System.Collections.Generic;

namespace Superfine.Unity
{
    public class SuperfineSDKUnityIAP
    {
        public static void LogIAPReceipt(Product product)
        {
            if (product == null)
            {
                Debug.LogWarning("Empty product");
                return;
            }

            string transactionId = product.transactionID;
            if (string.IsNullOrEmpty(transactionId))
            {
                Debug.LogWarning("Product has no transaction id");
            }

            string receipt = product.receipt;
            if (string.IsNullOrEmpty(receipt))
            {
                Debug.LogWarning("Product has no receipt");
                return;
            }

            try
            {
                var receipt_wrapper = (Dictionary<string, object>)MiniJson.JsonDecode(receipt);
                if (receipt_wrapper == null)
                {
                    Debug.LogWarning("Product receipt has no data");
                    return;
                }

                if (!receipt_wrapper.ContainsKey("Store"))
                {
                    Debug.LogWarning("Missing Store in product receipt");
                    return;
                }

                if (!receipt_wrapper.ContainsKey("Payload"))
                {
                    Debug.LogWarning("Missing Payload in product receipt");
                    return;
                }

                //Inject transaction id into Unity receipt
                receipt_wrapper["TransactionID"] = transactionId;

                SuperfineSDK.LogIAPReceipt_Unity(MiniJson.JsonEncode(receipt_wrapper));
            }
            catch (Exception e)
            {
                Debug.LogWarning("Error parsing product receipt: " + e.Message);
                return;
            }
        }
    }
}

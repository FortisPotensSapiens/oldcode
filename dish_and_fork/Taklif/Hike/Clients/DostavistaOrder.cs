namespace Hike.Clients
{
    public class DostavistaOrder
    {
        public string type { get; set; }
        public object order_id { get; set; }
        public object order_name { get; set; }
        public int vehicle_type_id { get; set; }
        public DateTime? created_datetime { get; set; }
        public DateTime? finish_datetime { get; set; }
        public DostavistaOrderStatus status { get; set; }
        public string status_description { get; set; }
        public string matter { get; set; }
        public int total_weight_kg { get; set; }
        public bool is_client_notification_enabled { get; set; }
        public bool is_contact_person_notification_enabled { get; set; }
        public int loaders_count { get; set; }
        public string backpayment_details { get; set; }
        public Point[] points { get; set; }
        public string payment_amount { get; set; }
        public string delivery_fee_amount { get; set; }
        public string intercity_delivery_fee_amount { get; set; }
        public string weight_fee_amount { get; set; }
        public string insurance_amount { get; set; }
        public string insurance_fee_amount { get; set; }
        public string loading_fee_amount { get; set; }
        public string money_transfer_fee_amount { get; set; }
        public string suburban_delivery_fee_amount { get; set; }
        public string overnight_fee_amount { get; set; }
        public string discount_amount { get; set; }
        public string backpayment_amount { get; set; }
        public string cod_fee_amount { get; set; }
        public string backpayment_photo_url { get; set; }
        public string itinerary_document_url { get; set; }
        public string waybill_document_url { get; set; }
     //   public string courier { get; set; }
        public bool is_motobox_required { get; set; }
        public string payment_method { get; set; }
        public string bank_card_id { get; set; }
    }
}

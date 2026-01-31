namespace Daf.DeliveryModule.Domain
{
    public class Point
    {
        public string? point_type { get; set; }
        public object? point_id { get; set; }
        public object? delivery_id { get; set; }
        public string? client_order_id { get; set; }
        public string? address { get; set; }
        public string? latitude { get; set; }
        public string? longitude { get; set; }
        public DateTime? required_start_datetime { get; set; }
        public DateTime? required_finish_datetime { get; set; }
        public object? arrival_start_datetime { get; set; }
        public object? arrival_finish_datetime { get; set; }
        public object? estimated_arrival_datetime { get; set; }
        public object? courier_visit_datetime { get; set; }
        public DostavistaContact? contact_person { get; set; }
        public string? taking_amount { get; set; }
        public string? buyout_amount { get; set; }
        public string? note { get; set; }
        public DostavistaPakage[] packages { get; set; } = new DostavistaPakage[0];
        public bool? is_cod_cash_voucher_required { get; set; }
        public object? place_photo_url { get; set; }
        public object? sign_photo_url { get; set; }
        public object? tracking_url { get; set; }
        public object? checkin { get; set; }
        public bool? is_order_payment_here { get; set; }
        public string? building_number { get; set; }
        public string? entrance_number { get; set; }
        public string? intercom_code { get; set; }
        public string? floor_number { get; set; }
        public string? apartment_number { get; set; }
        public object? invisible_mile_navigation_instructions { get; set; }
    }
}

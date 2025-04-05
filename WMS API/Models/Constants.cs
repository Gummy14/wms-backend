namespace WMS_API.Models
{
    public class Constants
    {
        public const int LOCATION_REGISTERED_AS_UNOCCUPIED = 110;
        public const int LOCATION_OCCUPIED = 111;
        public const int LOCATION_UNOCCUPIED = 112;
        //public const int LOCATION_SELECTED_FOR_ITEM_PUTAWAY = 113;

        public const int CONTAINER_REGISTERED_AS_NOT_IN_USE = 210;
        public const int CONTAINER_IN_USE = 211;
        public const int CONTAINER_NOT_IN_USE = 212;

        public const int ITEM_REGISTERED_WAITING_FOR_PUTAWAY = 310;

        public const int ITEM_PUTAWAY_INTO_LOCATION_COMPLETE = 410;

        public const int ORDER_REGISTERED_WAITING_FOR_ACKNOWLEDGEMENT = 510;
        public const int ITEM_ADDED_TO_ORDER = 511;
        public const int ORDER_ACKNOWLEDGED_PICKING_IN_PROGRESS = 520;
        public const int CONTAINER_SELECTED_FOR_PICKING = 521;
        public const int ITEM_PICKED_INTO_CONTAINER = 522;

        public const int BOX_REGISTERED = 610;
        public const int ITEM_PACKED_IN_BOX = 611;

        //public const int ORDER_PICKING_COMPLETED_WAITING_FOR_PACKAGING_SELECTION = 510;
        //public const int ORDER_SELECTED_FOR_PACKAGING_PACKAGING_IN_PROGRESS = 520;

        //public const int ORDER_PACKAGING_COMPLETED_WAITING_FOR_SHIPPING_SELECTION = 610;
        //public const int ORDER_SELECTED_FOR_SHIPPING_SHIPPING_PREP_IN_PROGRESS = 620;

        //public const int ORDER_SHIPPED = 710;
    }
}

namespace WMS_API.Models
{
    public class Constants
    {
        public const int CONTAINER_REGISTERED_AS_EMPTY = 110;
        public const int CONTAINER_FULL = 111;
        public const int CONTAINER_EMPTY = 112;

        public const int LOCATION_REGISTERED_AS_UNOCCUPIED = 810;
        public const int LOCATION_OCCUPIED = 811;
        public const int LOCATION_EMPTY = 812;

        public const int ITEM_REGISTERED_WAITING_FOR_PUTAWAY_SELECTION = 210;
        public const int ITEM_SELECTED_FOR_PUTAWAY_PUTAWAY_IN_PROGRESS = 220;

        public const int ITEM_PUTAWAY_INTO_CONTAINER_COMPLETE = 310;

        public const int ORDER_REGISTERED_WAITING_FOR_PICKING_SELECTION = 410;
        public const int ITEM_ADDED_TO_ORDER = 411;
        public const int ORDER_SELECTED_FOR_PICKING_PICKING_IN_PROGRESS = 420;
        public const int CONTAINER_SELECTED_FOR_PICKING = 421;
        public const int ITEM_PICKED_FROM_CONTAINER_BEFORE = 422;
        public const int ITEM_PICKED_FROM_CONTAINER_AFTER = 423;

        public const int ORDER_PICKING_COMPLETED_WAITING_FOR_PACKAGING_SELECTION = 510;
        public const int ORDER_SELECTED_FOR_PACKAGING_PACKAGING_IN_PROGRESS = 520;

        public const int ORDER_PACKAGING_COMPLETED_WAITING_FOR_SHIPPING_SELECTION = 610;
        public const int ORDER_SELECTED_FOR_SHIPPING_SHIPPING_PREP_IN_PROGRESS = 620;

        public const int ORDER_SHIPPED = 710;
    }
}

namespace WMS_API.Models
{
    public class Constants
    {
        public const int CONTAINER_REGISTERED = 1;
        public const int ITEM_REGISTERED_ADDED_TO_PUTAWAY_QUEUE = 2;
        public const int ITEM_SELECTED_FROM_PUTAWAY_QUEUE_PUTAWAY_IN_PROGRESS = 3;
        public const int ITEM_PUTAWAY_INTO_CONTAINER_COMPLETE = 4;
        public const int ITEM_ADDED_TO_ORDER = 5;
        public const int ITEM_PICKED_FROM_CONTAINER_BEFORE = 6;
        public const int ITEM_PICKED_FROM_CONTAINER_AFTER = 7;
        public const int ORDER_ADDED_TO_NEW_ORDERS_QUEUE_WAITING_TO_BE_SELECTED = 8;
        public const int ORDER_SELECTED_FROM_NEW_ORDERS_QUEUE_PICKING_IN_PROGRESS = 9;
        public const int ORDER_PICKING_COMPLETED_MOVING_tO_PACKAGING_QUEUE = 10;
        public const int ORDER_ADDED_TO_PACKAGING_QUEUE_WAITING_TO_BE_SELECTED = 11;
        public const int ORDER_SELECTED_FROM_PACKAGING_QUEUE_PACKAGING_IN_PROGRESS = 12;
        public const int ORDER_PACKAGING_COMPLETED_MOVING_TO_SHIPPING_QUEUE = 13;
        public const int ORDER_ADDED_SHIPPING_QUEUE_WAITING_TO_BE_SELECTED = 14;
        public const int ORDER_SELECTED_FROM_SHIPPING_QUEUE_SHIPPING_PREPERATION_IN_PROGRESS = 15;
        public const int ORDER_SHIPPED = 16;
    }
}

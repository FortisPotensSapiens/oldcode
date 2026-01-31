import { ORDER_VARIANT_PICKUP, ORDER_VARIANT_SHIPPING } from './constants';

export type OrderVariant = typeof ORDER_VARIANT_PICKUP | typeof ORDER_VARIANT_SHIPPING;

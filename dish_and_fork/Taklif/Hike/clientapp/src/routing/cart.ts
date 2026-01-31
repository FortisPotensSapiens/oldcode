import { joinPath } from '~/utils';

const CART_PART = 'cart';

const getCartPath = (): string => joinPath(CART_PART);

export { CART_PART, getCartPath };

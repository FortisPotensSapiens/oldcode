import { CartItem } from '~/contexts';

type CartCount = (items: CartItem[]) => number;

const cartCount: CartCount = (items) => items.reduce((sum, { amount }) => sum + amount, 0);

export { cartCount };

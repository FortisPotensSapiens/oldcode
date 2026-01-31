import { LOCALSTORE_CART_KEY } from './contants';
import { Cart } from './types';

type InitCart = () => Cart;

const initCart: InitCart = () => {
  const saved = window.localStorage.getItem(LOCALSTORE_CART_KEY);

  if (!saved) {
    return {};
  }

  try {
    const result: Cart = JSON.parse(saved);

    Object.values(result).forEach((value) => {
      value.updated = new Date(value.updated);
    });

    return result;
  } catch (e) {
    console.error('Error jy parsing saved cart:', e);

    return {};
  }
};

export default initCart;

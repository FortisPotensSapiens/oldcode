import constate from 'constate';

import useCartContext from './useCartContext';

const [CartProvider, useCartState, useCartActions] = constate(
  useCartContext,
  ([state]) => state,
  ([state, actions]) => actions,
);

export * from './types';
export { CartProvider, useCartActions, useCartState };

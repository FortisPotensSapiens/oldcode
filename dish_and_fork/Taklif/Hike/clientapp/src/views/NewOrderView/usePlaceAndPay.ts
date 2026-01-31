import { useCallback, useRef, useState } from 'react';

import { usePayAnyWayOrder, usePostNewOrder, usePostNewPickupOrder } from '~/api';
import { useCartActions } from '~/contexts';
import { CartItem } from '~/types';

import { FormData } from './types';
import { usePlaceOrder } from './usePlaceOrder';

type UsePlaceAndPayCall = (formData: FormData) => Promise<void>;

type UsePlaceAndPay = (
  items: CartItem[],
  clearCart: boolean,
) => [
  UsePlaceAndPayCall,
  {
    isError: boolean;
    isLoading: boolean;
    error: unknown;
  },
];

const usePlaceAndPay: UsePlaceAndPay = (items, clearCart) => {
  const [isLoading, setIsLoading] = useState(false);
  const { change } = useCartActions();
  const ref = useRef<string>();
  const placeShippingOrder = usePostNewOrder();
  const placePickupOrder = usePostNewPickupOrder();
  const payOrder = usePayAnyWayOrder();
  const [getOrderId] = usePlaceOrder(items);
  const [error, setError] = useState('');

  const call = useCallback<UsePlaceAndPayCall>(
    async (params) => {
      let orderId;

      setError('');

      try {
        orderId = await getOrderId(params);
      } catch (e) {
        // eslint-disable-next-line
        setError(e as any);
        return;
      }

      if (!orderId) {
        return;
      }

      let url = '';
      try {
        url = await payOrder.mutateAsync(orderId);
      } catch (e) {
        console.error(`Can't get the url to pay for the order ${ref.current}`);
        return;
      }

      setIsLoading(true);

      if (clearCart) {
        items.forEach((item) => change(item.merchandise, 0));
      }

      window.location.assign(url);
    },
    [getOrderId, payOrder, items, change, clearCart],
  );

  return [
    call,
    {
      error,
      isError: placePickupOrder.isError || placeShippingOrder.isError || payOrder.isError,
      isLoading: isLoading || placePickupOrder.isLoading || placeShippingOrder.isLoading || payOrder.isLoading,
    },
  ];
};

export { usePlaceAndPay };

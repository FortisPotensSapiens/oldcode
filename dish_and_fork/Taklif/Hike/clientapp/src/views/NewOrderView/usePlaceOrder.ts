import { useCallback, useRef } from 'react';

import { usePostNewOrder, usePostNewPickupOrder } from '~/api';
import { CartItem } from '~/types';

import { ORDER_VARIANT_PICKUP, ORDER_VARIANT_SHIPPING } from './OrderForm';
import { FormData } from './types';

type UsePlaceOrderCall = (formData: FormData) => Promise<string | undefined>;

type UsePlaceOrder = (items: CartItem[]) => [
  UsePlaceOrderCall,
  {
    isError: boolean;
    isLoading: boolean;
  },
];

const usePlaceOrder: UsePlaceOrder = (items) => {
  const ref = useRef<string>();
  const placeShippingOrder = usePostNewOrder();
  const placePickupOrder = usePostNewPickupOrder();

  const call = useCallback<UsePlaceOrderCall>(
    async ({
      apartmentNumber,
      comments,
      entrance,
      house,
      intercom,
      recipientFullName,
      recipientPhone,
      street,
      variant,
    }) => {
      if (ref.current) {
        return ref.current;
      }

      try {
        const payload = {
          comments,
          items: items.map(({ amount, merchandise }) => ({ amount, itemId: merchandise.id })),
          recipientFullName,
          recipientPhone,
        };

        switch (variant) {
          case ORDER_VARIANT_PICKUP:
            ref.current = await placePickupOrder.mutateAsync(payload);
            break;

          case ORDER_VARIANT_SHIPPING:
            ref.current = await placeShippingOrder.mutateAsync({
              ...payload,
              recipientAddress: { apartmentNumber, entrance, house, intercom, street },
            });
            break;

          default:
            console.error('Wrong value of order variant');
        }
      } catch (e) {
        console.error(e);
        throw e;
      }

      return ref.current;
    },
    [placePickupOrder, placeShippingOrder, items],
  );

  return [
    call,
    {
      error: placePickupOrder.error || placeShippingOrder.error,
      isError: placePickupOrder.isError || placeShippingOrder.isError,
      isLoading: placePickupOrder.isLoading || placeShippingOrder.isLoading,
    },
  ];
};

export { usePlaceOrder };
export type { UsePlaceOrder, UsePlaceOrderCall };

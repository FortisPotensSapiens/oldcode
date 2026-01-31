import { useCallback, useRef } from 'react';

import { usePostPlaceNewIndividualPickupOrder } from '~/api/v1/usePostPlaceNewIndividualPickupOrder';
import { usePostPlaceNewIndivudialOrder } from '~/api/v1/usePostPlaceNewIndivudialOrder';
import { AddressReadModel, OrderCreateModel } from '~/types';
import { OrderVariant } from '~/views/NewOrderView/OrderForm';
import { ORDER_VARIANT_PICKUP, ORDER_VARIANT_SHIPPING } from '~/views/NewOrderView/OrderForm/constants';

export type FormData = Pick<AddressReadModel, 'apartmentNumber' | 'entrance' | 'house' | 'intercom' | 'street'> &
  Pick<OrderCreateModel, 'comments' | 'recipientFullName' | 'recipientPhone'> & {
    variant: OrderVariant;
  };

type UsePlaceOrderCall = (formData: FormData) => Promise<string | undefined>;

type UsePlaceOrder = (offerId: string) => [
  UsePlaceOrderCall,
  {
    isError: boolean;
    isLoading: boolean;
  },
];

const usePlaceIndividualOrder: UsePlaceOrder = (offerId) => {
  const ref = useRef<string>();
  const placeShippingOrder = usePostPlaceNewIndivudialOrder();
  const placePickupOrder = usePostPlaceNewIndividualPickupOrder();

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
          offerId,
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
              recipientAddress: { apartmentNumber, entrance, house, intercom, street } as any,
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
    [offerId, placePickupOrder, placeShippingOrder],
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

export default usePlaceIndividualOrder;

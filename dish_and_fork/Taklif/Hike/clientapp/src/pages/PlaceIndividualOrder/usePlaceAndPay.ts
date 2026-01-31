import { useCallback, useRef, useState } from 'react';

import { usePayAnyWayOrder } from '~/api';
import { AddressReadModel, OrderCreateModel } from '~/types';
import { OrderVariant } from '~/views/NewOrderView/OrderForm';

import usePlaceIndividualOrder from './usePlaceIndividualOrder';

export type FormData = Pick<AddressReadModel, 'apartmentNumber' | 'entrance' | 'house' | 'intercom' | 'street'> &
  Pick<OrderCreateModel, 'comments' | 'recipientFullName' | 'recipientPhone'> & {
    variant: OrderVariant;
  };

type UsePlaceAndPayCall = (formData: FormData) => Promise<void>;

type UsePlaceAndPay = (offerId: string) => [
  UsePlaceAndPayCall,
  {
    isError: boolean;
    isLoading: boolean;
    error: unknown;
  },
];

const usePlaceAndPay: UsePlaceAndPay = (offerId: string) => {
  const [isLoading, setIsLoading] = useState(false);
  const ref = useRef<string>();
  const payOrder = usePayAnyWayOrder();
  const [getOrderId] = usePlaceIndividualOrder(offerId);
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

      window.location.assign(url);
    },
    [getOrderId, payOrder],
  );

  return [
    call,
    {
      error,
      isError: payOrder.isError,
      isLoading: isLoading || payOrder.isLoading,
    },
  ];
};

export { usePlaceAndPay };

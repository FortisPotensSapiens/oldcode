import { useCallback } from 'react';

import { usePayAnyWayOrder } from '~/api';

type UsePay = (
  id: string,
  onPay: (payload: boolean, orderId: string) => void,
) => [
  () => void,
  {
    isError: boolean;
    isLoading: boolean;
  },
];

const usePay: UsePay = (id, onPay) => {
  const { isError, isLoading, mutateAsync } = usePayAnyWayOrder();

  const call = useCallback(async () => {
    onPay(true, id);

    try {
      const data = await mutateAsync(id);
      window.location.replace(data);
    } catch (e) {
      onPay(false, id);
      console.error(e);
    }
  }, [id, onPay, mutateAsync]);

  return [call, { isError, isLoading }];
};

export { usePay };

import { useCallback } from 'react';

import { usePayAnyWayOrder } from '~/api';

type UsePayOrder = (id?: string) => [
  () => Promise<void>,
  {
    isError: boolean;
    isLoading: boolean;
    isSuccess: boolean;
  },
];

const usePayOrder: UsePayOrder = (id) => {
  const { isError, isLoading, isSuccess, mutateAsync } = usePayAnyWayOrder();

  const handler = useCallback(async () => {
    if (!id) {
      console.error("Сan't start the order payment process with an unknown ID");
      return;
    }

    try {
      window.location.assign(await mutateAsync(id));
    } catch (e) {
      console.error(`Can't get the url to pay for the order ${id}`);
    }
  }, [mutateAsync, id]);

  return [handler, { isError, isLoading, isSuccess }];
};

export { usePayOrder };
export type { UsePayOrder };

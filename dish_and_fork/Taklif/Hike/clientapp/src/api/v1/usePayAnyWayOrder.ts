import { useMutation, UseMutationResult } from 'react-query';

import { extractData } from '~/utils';

import { useAxios } from './axios';

type UsePayAnyWayOrder = () => UseMutationResult<string, unknown, string>;

const usePayAnyWayOrder: UsePayAnyWayOrder = () => {
  const { get } = useAxios();

  return useMutation((orderId) => extractData(get(`/pay-any-way/pay-order/${orderId}`)));
};

export { usePayAnyWayOrder };
export type { UsePayAnyWayOrder };

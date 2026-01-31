import { useMutation, UseMutationResult } from 'react-query';

import { extractData } from '~/utils';

import { useAxios } from './axios';

type UsePutAcceptTermOfService = () => UseMutationResult<unknown, unknown, unknown, unknown>;

const usePutAcceptTermOfService: UsePutAcceptTermOfService = () => {
  const { put } = useAxios();

  return useMutation((data) => extractData(put(`/accept-terms-of-service`, data)));
};

export { usePutAcceptTermOfService };
export type { UsePutAcceptTermOfService };

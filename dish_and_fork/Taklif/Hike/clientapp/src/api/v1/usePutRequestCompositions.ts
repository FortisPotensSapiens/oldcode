import { useMutation, UseMutationResult } from 'react-query';

import { extractData } from '~/utils';

import { useAxios } from './axios';

type UsePutRequestCompositions = () => UseMutationResult<string, unknown, { goodId: string }, unknown>;

const usePutRequestCompositions: UsePutRequestCompositions = () => {
  const { put } = useAxios();

  return useMutation((data) => extractData(put(`/goods/${data.goodId}/request-composition`)));
};

export { usePutRequestCompositions };
export type { UsePutRequestCompositions };

import { useMutation, UseMutationResult } from 'react-query';

import { extractData } from '~/utils';

import { useAxios } from './axios';

type UsePutPublishGood = () => UseMutationResult<string, unknown, { goodId: string }, unknown>;

const usePutPublishGood: UsePutPublishGood = () => {
  const { put } = useAxios();

  return useMutation((data) => extractData(put(`/seller/goods/${data.goodId}/publish`)));
};

export { usePutPublishGood };
export type { UsePutPublishGood };

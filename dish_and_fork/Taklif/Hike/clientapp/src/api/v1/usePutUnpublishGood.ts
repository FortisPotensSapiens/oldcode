import { useMutation, UseMutationResult } from 'react-query';

import { extractData } from '~/utils';

import { useAxios } from './axios';

type UsePutUnpublishGood = () => UseMutationResult<string, unknown, { goodId: string }, unknown>;

const usePutUnpublishGood: UsePutUnpublishGood = () => {
  const { put } = useAxios();

  return useMutation((data) => extractData(put(`/seller/goods/${data.goodId}/unpublish`)));
};

export { usePutUnpublishGood };
export type { UsePutUnpublishGood };

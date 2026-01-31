import { useMutation, UseMutationResult } from 'react-query';

import { MerchRatingCreateModel } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UsePostAddProductFeedback = () => UseMutationResult<MerchRatingCreateModel, unknown, MerchRatingCreateModel>;

const usePostAddProductFeedback: UsePostAddProductFeedback = () => {
  const { post } = useAxios();

  return useMutation((data) => extractData(post(`merch-ratings`, data)));
};

export { usePostAddProductFeedback };
export type { UsePostAddProductFeedback };

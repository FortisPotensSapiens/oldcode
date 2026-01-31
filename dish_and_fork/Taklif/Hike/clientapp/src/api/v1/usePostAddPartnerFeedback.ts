import { useMutation, UseMutationResult } from 'react-query';

import { MerchRatingCreateModel } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UsePostAddPartnerFeedback = () => UseMutationResult<MerchRatingCreateModel, unknown, MerchRatingCreateModel>;

const usePostAddPartnerFeedback: UsePostAddPartnerFeedback = () => {
  const { post } = useAxios();

  return useMutation((data) => extractData(post(`partner-ratings`, data)));
};

export { usePostAddPartnerFeedback };

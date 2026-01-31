import { useMutation, UseMutationResult } from 'react-query';

import { UseQueryError } from '~/types/query';
import { OfferCommentCreateModel } from '~/types/swagger';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UsePostNewIndividualOrderComment = () => UseMutationResult<
  string,
  UseQueryError,
  OfferCommentCreateModel,
  unknown
>;

export const usePostNewIndividualOrderComment: UsePostNewIndividualOrderComment = () => {
  const { post } = useAxios();

  return useMutation((data) => extractData(post('/individual-applications/offers/comments', data)));
};

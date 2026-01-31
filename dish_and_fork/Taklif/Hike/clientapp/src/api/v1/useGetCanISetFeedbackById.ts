import { useQuery, UseQueryResult } from 'react-query';

import { extractData } from '~/utils';

import { useAxios } from './axios';

type UseGetCanISetFeedbackById = (id: string) => UseQueryResult<boolean>;

const useGetCanISetFeedbackById: UseGetCanISetFeedbackById = (id) => {
  const { get } = useAxios();

  return useQuery(['apiV1UseCanISetFeedbackById', id], () => extractData(get(`can-i-set-rating-to-merch/${id}`)));
};

export { useGetCanISetFeedbackById };
export type { UseGetCanISetFeedbackById };

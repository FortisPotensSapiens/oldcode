import { useQuery, UseQueryResult } from 'react-query';

import { extractData } from '~/utils';

import { useAxios } from './axios';

type UseGetCanISetFeedbackPartnerById = (id: string) => UseQueryResult<boolean>;

const useGetCanISetFeedbackPartnerById: UseGetCanISetFeedbackPartnerById = (id) => {
  const { get } = useAxios();

  return useQuery(['UseGetCanISetFeedbackPartnerById', id], () =>
    extractData(get(`can-i-set-rating-to-partner/${id}`)),
  );
};

export { useGetCanISetFeedbackPartnerById };

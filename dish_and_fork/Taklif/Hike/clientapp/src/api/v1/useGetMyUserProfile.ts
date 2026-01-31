import { useQuery, UseQueryOptions, UseQueryResult } from 'react-query';

import { UseQueryError, UserProfileDetailsReadModel } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UseGetMyUserProfile = (
  options?: UseQueryOptions<UserProfileDetailsReadModel, UseQueryError>,
) => UseQueryResult<UserProfileDetailsReadModel, UseQueryError>;

const useGetMyUserProfile: UseGetMyUserProfile = (options) => {
  const { get } = useAxios();

  return useQuery<UserProfileDetailsReadModel, UseQueryError>(
    ['apiV1GetMyUserProfile'],
    () => extractData(get(`/user-profile/my?timestamp=${Date.now()}`)),
    options,
  );
};

export { useGetMyUserProfile };
export type { UseGetMyUserProfile };

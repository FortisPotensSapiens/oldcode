import { useQuery, UseQueryResult } from 'react-query';

import { GlobaSettingsModel } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UseGetAdminGlobalSettings = () => UseQueryResult<GlobaSettingsModel>;

const useGetAdminGlobalSettings: UseGetAdminGlobalSettings = () => {
  const { get } = useAxios();

  return useQuery(['useGetAdminGlobalSettings'], () => extractData(get('/config/admin/global-settings')));
};

export { useGetAdminGlobalSettings };
export type { UseGetAdminGlobalSettings };

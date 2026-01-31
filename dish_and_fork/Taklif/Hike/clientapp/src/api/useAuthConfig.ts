import axios from 'axios';
import { UserManagerSettings } from 'oidc-client';
import { useQuery, UseQueryResult } from 'react-query';

import { useConfig } from '~/contexts';
import { extractData } from '~/utils';

type UseAuthConfig = () => UseQueryResult<UserManagerSettings>;

const useAuthConfig: UseAuthConfig = () => {
  const { api } = useConfig();

  return useQuery(['authConfig'], () => extractData(axios.get(`${api.authConfigUrl}/${api.authClientId}`)));
};

export { useAuthConfig };
export type { UseAuthConfig };

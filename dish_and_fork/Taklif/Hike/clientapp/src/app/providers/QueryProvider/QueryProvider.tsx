import { FC } from 'react';
import { QueryClientProvider } from 'react-query';

import { useForbidden } from './useForbidden';
import { useQueryClient } from './useQueryClient';

const QueryProvider: FC = ({ children }) => {
  const onForbidden = useForbidden();
  const queryClient = useQueryClient({ onForbidden });

  return <QueryClientProvider client={queryClient}>{children}</QueryClientProvider>;
};

export { QueryProvider };

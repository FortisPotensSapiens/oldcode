import { useCallback } from 'react';
import { useNavigate } from 'react-router-dom';

import { generatePartnerProductPath } from '~/routing';

type UseActions = (productId: string) => {
  edit: () => void;
  publish: () => undefined;
  remove: () => undefined;
  unpublish: () => undefined;
};

const useActions: UseActions = (productId) => {
  const navigate = useNavigate();
  const edit = useCallback(() => navigate(generatePartnerProductPath({ productId })), [productId, navigate]);
  const publish = useCallback(() => undefined, []);
  const remove = useCallback(() => undefined, []);
  const unpublish = useCallback(() => undefined, []);

  return { edit, publish, remove, unpublish };
};

export { useActions };
export type { UseActions };

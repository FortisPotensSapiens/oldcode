import { MouseEventHandler, useCallback } from 'react';

import { useCartActions } from '~/contexts';
import { MerchandiseReadModel } from '~/types';

type UseClick = (merchandise: MerchandiseReadModel) => MouseEventHandler<HTMLButtonElement>;

const useClick: UseClick = (merchandise) => {
  const { increase } = useCartActions();

  return useCallback(
    (event) => {
      event.preventDefault();
      event.stopPropagation();
      increase(merchandise);
    },
    [increase, merchandise],
  );
};

export default useClick;

import { MouseEventHandler, useCallback } from 'react';

type UseClick = (onClick?: MouseEventHandler<HTMLAnchorElement>) => MouseEventHandler<HTMLAnchorElement>;

const useClick: UseClick = (onClick) =>
  useCallback<MouseEventHandler<HTMLAnchorElement>>(
    (event) => {
      if (onClick) {
        event.preventDefault();

        onClick(event);
      }
    },
    [onClick],
  );

export { useClick };
export type { UseClick };

import { FC, MouseEventHandler } from 'react';

import { useDownSm } from '~/hooks';
import { FileReadModel } from '~/types';

import StyledContainer from './StyledContainer';
import StyledControl from './StyledControl';
import StyledItem from './StyledItem';
import { Props } from './types';

type ImagesThumbsProps = Props & {
  selected?: number;
  items?: FileReadModel[];
  onClick: MouseEventHandler<HTMLButtonElement>;
};

const ImagesThumbs: FC<ImagesThumbsProps> = ({ items, onClick, selected, vertical }) => {
  const isSmall = useDownSm();

  if (!items?.length) {
    return null;
  }

  return (
    <StyledContainer vertical={vertical}>
      {items.map(
        ({ id, path }, index) =>
          path && (
            // TDOD сделать прокрутку для большого количества картинок вместо оганичения ко количеству элементов
            <StyledItem key={id} hideMd={index > 3} vertical={vertical}>
              <StyledControl
                data-index={index}
                onClick={onClick}
                style={isSmall ? undefined : { backgroundImage: `url(${path})` }}
              />
            </StyledItem>
          ),
      )}
    </StyledContainer>
  );
};

export { ImagesThumbs };

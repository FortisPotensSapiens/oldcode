import isMobile from 'is-mobile';
import { HTML5toTouch } from 'rdndmb-html5-to-touch';
import { FC } from 'react';
import { DndProvider } from 'react-dnd-multi-backend';
import { usePreview } from 'react-dnd-preview';

const DndPreview = () => {
  const preview = usePreview();
  if (!preview.display || !isMobile()) {
    return null;
  }
  const { item, style } = preview;

  style.zIndex = 999;

  return (
    <div style={style}>
      <img
        src={(item as any).image}
        alt=""
        width="100px"
        height="100px"
        style={{
          objectFit: 'contain',
        }}
      />
    </div>
  );
};

const DndAppProvider: FC = ({ children }) => {
  return (
    <DndProvider options={HTML5toTouch}>
      {children}
      <DndPreview />
    </DndProvider>
  );
};

export { DndAppProvider };

import React, { PropTypes as RPT, PureComponent as Component } from 'react';
import Slider from 'rc-slider';
import { connect } from 'react-redux';
import { LineChart } from 'react-d3-components';
import { putImage, putReference } from '../../common/api/actions';

@connect(state => ({
  sessionId: state.app.get('sessionId')
}), { putImage, putReference })
export default class ImagePair extends Component {

  static propTypes = {
    cannyTreshodLinking: RPT.number,
    cannyTreshold: RPT.number,
    contourUrl: RPT.string,
    id: RPT.string,
    imageUrl: RPT.string,
    isSelected: RPT.bool,
    onClick: RPT.func,
    pairType: RPT.oneOf(['reference', 'image']),
    putImage: RPT.func,
    putReference: RPT.func,
    sessionId: RPT.string,
    timeline: RPT.array
  }

  render() {
    const { id, imageUrl, contourUrl, cannyTreshodLinking, cannyTreshold, putImage, putReference, pairType, sessionId, isSelected, onClick, timeline } = this.props;
    const wrapperStyle = isSelected ? { ...styles.imageWrapper, ...styles.selected } : styles.imageWrapper;
    const parts = imageUrl.split('/');
    const fileName = parts[parts.length - 1];

    const data = [{
      label: `${fileName}`,
      values: Object.keys(timeline).reduce((prev2, key) =>
        [...prev2, { x: parseInt(key, 10), y: timeline[key] }]
      , [])
    }];

    return (
      <div style={wrapperStyle} onClick={onClick ? () => onClick() : () => {}}>
        <span>{fileName}</span>
        <img src={imageUrl} style={styles.image} />
        <img src={contourUrl} style={styles.image} />
        <LineChart
          data={data}
          width={200}
          height={100}
        />
        <p>cannyThreshold</p>
        <Slider
          max={250}
          min={5}
          step={5}
          onAfterChange={(x) => pairType === 'reference'
            ? putReference(id, x, cannyTreshodLinking)
            : putImage(sessionId, id, x, cannyTreshodLinking)}
          defaultValue={cannyTreshold}
        />
        <p>cannyThresholdLinking</p>
        <Slider
          max={250}
          min={5}
          step={5}
          onAfterChange={(x) => pairType === 'reference'
            ? putReference(id, cannyTreshold, x)
            : putImage(sessionId, id, cannyTreshold, x)}
          defaultValue={cannyTreshodLinking}
        />
      </div>
    );
  }
}

const styles = {
  imageWrapper: {
    width: '250px',
    float: 'left',
    marginLeft: '20px',
    marginTop: '20px',
    padding: '10px',
    boxSizing: 'border-box',
    border: '5px solid white',
  },
  image: {
    width: '210px'
  },
  selected: {
    border: '5px solid #5b5a5b',
    backgroundColor: '#c6c6c6'
  }
}

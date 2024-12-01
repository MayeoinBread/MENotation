using MENotation.ScoreRendering;
using MENotation.XmlHandling.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;

namespace MENotation.XmlHandling
{
    public static class XmlParser
    {
        public static Score ParseXmlToScore(XmlDocument xmlDoc)
        {
            var score = new Score
            {
                Work = GetScoreWork(xmlDoc.SelectSingleNode("score-partwise/work")),
                Identification = GetScoreIdentification(xmlDoc.SelectSingleNode("score-partwise/identification")),
                PartList = GetScorePartsList(xmlDoc.SelectSingleNode("score-partwise"))
            };

            return score;
        }

        private static Work GetScoreWork(XmlNode workNode)
        {
            var work = new Work();

            var numberNode = workNode.SelectSingleNode("work-number");
            var titleNode = workNode.SelectSingleNode("work-title");

            if (numberNode != null) work.Number = numberNode.InnerText;
            if (titleNode != null) work.Title = titleNode.InnerText;

            return work;
        }

        private static Identification GetScoreIdentification(XmlNode identificationNode)
        {
            if (identificationNode == null) return null;

            var identification = new Identification();

            var creatorNodes = identificationNode.SelectNodes("creator");
            var rightsNodes = identificationNode.SelectNodes("rights");
            var sourceNodes = identificationNode.SelectNodes("source");
            var encodingNode = identificationNode.SelectSingleNode("encoding");


            identification.Creator = GetListMultipleNodes(creatorNodes, "type");
            identification.Rights = GetListMultipleNodes(rightsNodes);
            identification.Source = GetListMultipleNodes(sourceNodes);
            identification.Encoding = GetScoreEncoding(encodingNode);

            return identification;
        }

        private static List<string> GetListMultipleNodes(XmlNodeList nodeList, string attributeName="")
        {
            if (nodeList == null || nodeList.Count == 0) return null;
            
            var list = new List<string>();

            if (nodeList != null)
            {
                foreach(XmlNode nodeListNode in nodeList)
                {
                    if (nodeListNode.Attributes != null && attributeName.Length > 0)
                    {
                        list.Add(string.Format("{0}: {1}",
                            nodeListNode.Attributes[attributeName].InnerText,
                            nodeListNode.InnerText));
                    }
                    else
                    {
                        list.Add(nodeListNode.InnerText);
                    }
                }
            }

            return list;
        }

        private static Classes.Encoding GetScoreEncoding(XmlNode encodingNode)
        {
            if (encodingNode == null) return null;
            return new Classes.Encoding
            {
                Date = GetListMultipleNodes(encodingNode.SelectNodes("encoding-date")),
                Encoder = GetListMultipleNodes(encodingNode.SelectNodes("encoder")),
                Software = GetListMultipleNodes(encodingNode.SelectNodes("software")),
                Description = GetListMultipleNodes(encodingNode.SelectNodes("encoding-description"))
            };
        }

        private static List<Part> GetScorePartsList(XmlNode partwiseNode)
        {
            var list = new List<Part>();

            var nodeNameList = new List<string>();

            var partsNodes = partwiseNode.SelectNodes("part-list/score-part");
            if (partsNodes != null)
            {
                foreach(XmlNode partsNode in partsNodes)
                {
                    var part = new Part
                    {
                        // part-name + id is required in spec
                        Name = partsNode.SelectSingleNode("part-name").InnerText,
                        Id = partsNode.Attributes["id"].InnerText
                    };

                    // Get the part for this ID, then go through the measures
                    var partMeasures = partwiseNode.SelectNodes("part[@id='" + part.Id + "']/measure");
                    foreach(XmlNode measureNode in partMeasures)
                    {
                        var measure = new Measure
                        {
                            // Required, not specifically a number
                            Number = measureNode.Attributes["number"].InnerText,
                        };

                        foreach(XmlNode childNode in measureNode.ChildNodes)
                        {
                            var element = MeasureElementFactory.CreateElement(childNode);
                            measure.Elements.Add(element);
                        }

                        //var measureNoteNodes = measureNode.SelectNodes("note");
                        //foreach (XmlNode measureNoteNode in measureNoteNodes)
                        //{
                        //    measure.Notes.Add(GetNoteFromNode(measureNoteNode));
                        //}

                        part.Measures.Add(measure);
                    }

                    list.Add(part);
                }
            }

            foreach (var item in nodeNameList)
            {
                Debug.WriteLine(item);
            }

            return list;
        }

        public static Attributes GetMeasureAttributesForMeasure(XmlNode measureAttributesNode)
        {
            if (measureAttributesNode == null) return null;
            try
            {
                var measureAttributes = new Attributes
                {
                    // 0 or more time
                    Key = GetKeyForMeasure(measureAttributesNode.SelectSingleNode("key")),
                    // 0 or more times
                    Time = GetTimeForMeasure(measureAttributesNode.SelectSingleNode("time")),
                    // 0 or more times
                    Clef = GetClefForMeasure(measureAttributesNode.SelectSingleNode("clef")),
                    // 0 or more times
                    StaffDetails = GetStaffDetailsForMeasure(measureAttributesNode.SelectSingleNode("staff-details")),
                    // 0 or more times
                    Transpose = GetTransposeDetailsForMeasure(measureAttributesNode.SelectSingleNode("transpose"))
                };

                // Optional, single nodes
                var divisionsNode = measureAttributesNode.SelectSingleNode("divisions");
                var stavesNode = measureAttributesNode.SelectSingleNode("staves");
                if (divisionsNode != null)
                {
                    measureAttributes.Divisions = int.Parse(divisionsNode.InnerText);
                }
                if (stavesNode != null) measureAttributes.Staves = int.Parse(stavesNode.InnerText);

                return measureAttributes;
            }
            catch
            {
                Debug.WriteLine("Error parsing MeasureAttributes nodes");
                return null;
            }
        }

        public static Backup GetBackupForNode(XmlNode node)
        {
            return new Backup
            {
                Duration = int.Parse(node.SelectSingleNode("duration").InnerText)
            };
        }

        public static Forward GetForwardForNode(XmlNode node)
        {
            return new Forward
            {
                Duration = int.Parse(node.SelectSingleNode("duration").InnerText)
            };
        }

        public static Direction GetDirectionAttributesForMeasure(XmlNode directionNode)
        {
            if (directionNode == null) return null;
            try
            {
                var direction = new Direction();

                if (directionNode.Attributes != null && directionNode.Attributes.Count > 0)
                {
                    if (directionNode.Attributes.GetNamedItem("placement") != null)
                    {
                        direction.Placement = directionNode.Attributes["placement"].InnerText;
                    }
                }

                var per_min = directionNode.SelectSingleNode("direction-type/metronome/per-minute");
                if (per_min != null)
                {
                    direction.PerMinute = int.Parse(per_min.InnerText);
                }

                var beatNode = directionNode.SelectSingleNode("direction-type/metronome/beat-unit");

                if (beatNode != null)
                {
                    if (Enum.TryParse(beatNode.InnerText, true, out NoteType noteType))
                    {
                        direction.BeatUnit = noteType;
                    }
                    else if (beatNode.InnerText == "16th")
                    {
                        direction.BeatUnit = NoteType.sixteenth;
                    }
                    else
                    {
                        direction.BeatUnit = NoteType.unknown;
                    }
                }
                return direction;
            }
            catch
            {
                Debug.WriteLine("Error parsing Direction nodes");
                return null;
            }
        }

        private static Key GetKeyForMeasure(XmlNode measureKeyNode)
        {
            if (measureKeyNode == null) return null;
            try
            {
                // Fifths should always be present in key
                var key = new Key
                {
                    Fifths = int.Parse(measureKeyNode.SelectSingleNode("fifths").InnerText)
                };
                
                // Mode is optional
                var modeNode = measureKeyNode.SelectSingleNode("mode");
                if (modeNode != null) key.Mode = modeNode.InnerText;

                return key;
            }
            catch
            {
                Debug.WriteLine("Error parsing Key nodes");
                return null;
            }
        }

        private static Time GetTimeForMeasure(XmlNode measureTimeNode)
        {
            if (measureTimeNode == null) return null;
            try
            {
                // Beats and BeatType should always be present
                return new Time
                {
                    Beats = int.Parse(measureTimeNode.SelectSingleNode("beats").InnerText),
                    BeatType = int.Parse(measureTimeNode.SelectSingleNode("beat-type").InnerText)
                };
            }
            catch
            {
                Debug.WriteLine("Error parsing Time nodes");
                return new Time();
            }
        }

        private static Clef GetClefForMeasure(XmlNode measureClefNode)
        {
            if (measureClefNode == null) return null;
            try
            {
                var clef = new Clef
                {
                    Sign = measureClefNode.SelectSingleNode("sign").InnerText
                };

                var linesNode = measureClefNode.SelectSingleNode("line");
                if (linesNode != null) clef.Line = int.Parse(linesNode.InnerText);

                return clef;
            }
            catch
            {
                Debug.WriteLine("Error parsing Clef nodes");
                return new Clef();
            }
        }

        private static StaffDetails GetStaffDetailsForMeasure(XmlNode measureStaffDetailsNode)
        {
            if (measureStaffDetailsNode == null) return null;
            try
            {
                return new StaffDetails
                {
                    StaffLines = int.Parse(measureStaffDetailsNode.SelectSingleNode("staff-lines").InnerText)
                };
            }
            catch
            {
                Debug.WriteLine("Error parsing StaffDetails nodes");
                return new StaffDetails();
            }
        }

        private static Transpose GetTransposeDetailsForMeasure(XmlNode measureTransposeNode)
        {
            if (measureTransposeNode == null) return null;
            try
            {
                var transpose = new Transpose
                {
                    // Required
                    Chromatic = int.Parse(measureTransposeNode.SelectSingleNode("chromatic").InnerText)
                };

                // Optional
                var diatonicNode = measureTransposeNode.SelectSingleNode("diatonic");
                var octaveChangeNode = measureTransposeNode.SelectSingleNode("octave-change");

                if (diatonicNode != null) transpose.Diatonic = int.Parse(diatonicNode.InnerText);
                if (octaveChangeNode != null) transpose.OctaveChange = int.Parse(octaveChangeNode.InnerText);

                return transpose;
            }
            catch
            {
                Debug.WriteLine("Error parsing Transpose nodes");
                return new Transpose();
            }
        }

        public static Note GetNoteFromNode(XmlNode noteNode)
        {
            if (noteNode == null) return null;
            try
            {
                var note = new Note
                {
                    Duration = int.Parse(noteNode.SelectSingleNode("duration").InnerText),
                    Pitch = GetPitchForNote(noteNode.SelectSingleNode("pitch")),
                    Lyric = GetLyricForNote(noteNode.SelectNodes("lyric")),
                    Beam = GetBeamForNote(noteNode.SelectNodes("beam"))
                };

                if (note.Pitch != null)
                {
                    note.staveLine = NotePositionCalculator.GetNoteLineOffset(note.Pitch);
                }

                var voiceNode = noteNode.SelectSingleNode("voice");
                var typeNode = noteNode.SelectSingleNode("type");
                var restNode = noteNode.SelectSingleNode("rest");
                var accidentalNode = noteNode.SelectSingleNode("accidental");
                var chordNode = noteNode.SelectSingleNode("chord");
                var dotNode = noteNode.SelectSingleNode("dot");

                if (voiceNode != null)
                {
                    note.Voice = voiceNode.InnerText;
                }

                if (typeNode != null)
                {
                    if (Enum.TryParse(typeNode.InnerText, true, out NoteType noteType))
                    {
                        note.Type = noteType;
                    }
                    else if (typeNode.InnerText == "16th")
                    {
                        note.Type = NoteType.sixteenth;
                    }
                    else
                    {
                        note.Type = NoteType.unknown;
                    }
                }
                
                if (restNode != null)
                {
                    note.IsRest = true;
                }

                if (chordNode != null)
                {
                    note.IsChord = true;
                }

                if (dotNode != null)
                {
                    note.IsDot = true;
                }

                if (accidentalNode != null)
                {
                    if (Enum.TryParse(accidentalNode.InnerText, true, out AccidentalType accidentalType))
                    {
                        note.Accidental = accidentalType;
                    }
                }

                return note;
            }
            catch
            {
                Debug.WriteLine("Error parsing Note nodes");
                return new Note();
            }
        }

        private static Pitch GetPitchForNote(XmlNode pitchNode)
        {
            if (pitchNode == null) return null;
            try
            {
                var pitch = new Pitch
                {
                    Step = pitchNode.SelectSingleNode("step").InnerText[0],
                    Octave = int.Parse(pitchNode.SelectSingleNode("octave").InnerText)
                };

                var alterNode = pitchNode.SelectSingleNode("alter");
                if (alterNode != null) pitch.Alter = int.Parse(alterNode.InnerText);

                return pitch;
            }
            catch
            {
                Debug.WriteLine("Error parsing Pitch nodes");
                return new Pitch();
            }


        }

        private static Lyric GetLyricForNote(XmlNodeList lyricNodes)
        {
            if (lyricNodes == null || lyricNodes.Count == 0) return null;
            
            Lyric lyric = new();
            foreach (XmlNode lyricNode in lyricNodes)
            {
                lyric.Number.Add(int.Parse(lyricNode.Attributes["number"].InnerText));
                lyric.Text.Add(lyricNode.SelectSingleNode("text").InnerText);

                var syllabicNode = lyricNode.SelectSingleNode("syllabic");
                if (syllabicNode != null)
                {
                    if (Enum.TryParse(syllabicNode.InnerText, true, out Syllabic syllabic))
                    {
                        lyric.Syllabic.Add(syllabic);
                    }
                    else
                    {
                        // Fallback if not specified
                        lyric.Syllabic.Add(Syllabic.single);
                    }
                }
            }
            return lyric;
        }

        private static Beam GetBeamForNote(XmlNodeList beamNodes)
        {
            if (beamNodes == null || beamNodes.Count == 0) return null;
            
            Beam beam = new();
            foreach (XmlNode beamNode in beamNodes)
            {
                beam.Number.Add(int.Parse(beamNode.Attributes["number"].InnerText));
                beam.Value.Add(beamNode.InnerText);
            }
            return beam;
        }
    }
}
